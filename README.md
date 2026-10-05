# WebProject

**🔗 Canlı demo: <!-- LIVE_URL -->_(yayına alındığında buraya eklenecek)_<!-- /LIVE_URL -->**

Denemek için hazır hesap: `alice` / `Pass123$` (normal kullanıcı). Admin paneli herkese açık değildir.

.NET 6 tabanlı bir e-ticaret mikroservis mimarisi: IdentityServer4 ile kimlik doğrulama, Ocelot API Gateway, ve Catalog/Basket/Order/Discount/Cargo/Comment/Payment/Message/Image/Recommendation gibi bağımsız mikroservisler, hepsi ortak bir MVC frontend (`WebProject.WebUI`) tarafından tüketiliyor.

## Mimari

```
WebProject.WebUI (5083/7177)  ->  Ocelot Gateway (5000)  ->  Mikroservisler (7070-7080)
                               \->  IdentityServer4 (5001)
```

| Servis | Port (http) | Veritabanı |
|---|---|---|
| IdentityServer | 5001 | PostgreSQL: `WebProjectIdentityDb` |
| Ocelot Gateway | 5000 | - |
| Catalog | 7070 | MongoDB: `WebProjectCatalogDb` + PostgreSQL: `WebProjectOrderDb` (okuma) |
| Discount | 7071 | PostgreSQL: `WebProjectDiscountDb` |
| Order | 7072 | PostgreSQL: `WebProjectOrderDb` |
| Cargo | 7073 | PostgreSQL: `WebProjectCargoDb` |
| Basket | 7074 | Redis |
| Comment | 7075 | PostgreSQL: `WebProjectCommentDb` |
| Payment | 7076 | - |
| Image | 7077 | - |
| Message | 7078 | PostgreSQL: `WebProjectMessageDb` |
| Recommendation | 7080 | MongoDB: `WebProjectCatalogDb` |
| RabbitMQMessage, SignalRRealTime, Images.WebUI (GCP), RapidApiWebUI | - | opsiyonel, ana WebUI akışı için gerekli değil |
| **WebUI (frontend)** | **5083 (http) / 7177 (https)** | - |

Tüm mikroservisler `IdentityServerUrl` üzerinden JWT doğrular ve trafik Ocelot Gateway üzerinden `http://localhost:5000/services/<servis>/...` şeklinde yönlenir.

## Gereksinimler

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (PostgreSQL, MongoDB, Redis, RabbitMQ için)

## Hızlı başlangıç (her şey Docker'da)

Önce gizli değerleri içeren `.env` dosyasını oluşturun (git'e girmez):

```bash
cp .env.example .env
# POSTGRES_PASSWORD, CLIENT_SECRET ve SEED_ADMIN_PASSWORD için rastgele değerler yazın:
#   openssl rand -base64 24
```

Tüm stack'i (11 .NET servisi + PostgreSQL, MongoDB, Redis) tek komutla ayağa kaldırır:

```bash
docker compose -f docker-compose.prod.yml up -d --build
```

İlk build 10-20 dakika sürer. Ardından IdentityServer'ı bir kez seed edin (`bob` admin parolası `.env`'deki `SEED_ADMIN_PASSWORD`'dür, boşsa konsola yazılır):

```bash
docker compose -f docker-compose.prod.yml run --rm identityserver dotnet WebProject.IdentityServer.dll /seed
```

İsteğe bağlı olarak örnek katalog verisini yükleyin:

```bash
docker cp scripts/seed_catalog.js $(docker compose -f docker-compose.prod.yml ps -q mongodb):/tmp/seed_catalog.js
docker compose -f docker-compose.prod.yml exec mongodb mongosh --quiet /tmp/seed_catalog.js
```

Sonra <http://localhost> adresini açın. Servislerin veritabanı migration'ları
açılışta otomatik uygulanır. Toplam bellek kullanımı ~550 MB'dır.

Aşağıdaki bölüm ise servisleri Docker yerine doğrudan `dotnet run` ile
çalıştırmak (geliştirme yaparken) içindir.

## Kurulum (geliştirme: servisler yerelde)

### 1) Altyapıyı ayağa kaldır

Repo kökünde:

```bash
docker compose up -d
```

Bu, PostgreSQL (5432), MongoDB (27017), Redis (6379) ve RabbitMQ'yu (5672, yönetim paneli 15672) başlatır.

### 2) HTTPS geliştirme sertifikasını güven listesine ekle (tek seferlik)

```bash
dotnet dev-certs https --trust
```

macOS bunun için parola/Touch ID isteyecektir. Bu adım atlanırsa tarayıcı `https://localhost:7177` gibi adreslerde sertifika hatası verir (http üzerinden erişim yine çalışır).

### 3) IdentityServer veritabanını oluştur ve seed et

```bash
cd IdentityServer/WebProject.IdentityServer
dotnet run -- /seed
```

Bu iki kullanıcı ve bir "Admin" rolü oluşturur:

| Kullanıcı | Şifre | Rol |
|---|---|---|
| `alice` | `Pass123$` (demo) | normal kullanıcı |
| `bob` | `SEED_ADMIN_PASSWORD` ortam değişkeni; verilmezse rastgele üretilip konsola **bir kez** yazılır | **Admin** (`/Admin/*` alanına erişebilir) |

Yerelde sabit bir admin parolası istersen: `SEED_ADMIN_PASSWORD='SenYaz1!' dotnet run -- /seed`

### 4) Tüm servisleri başlat

Her biri ayrı bir terminalde (veya arka planda) çalıştırılmalı — her servis kendi veritabanı migration'ını ilk açılışta otomatik uygular:

```bash
# IdentityServer
cd IdentityServer/WebProject.IdentityServer && dotnet run

# API Gateway
cd ApiGateway/WebProject.OcelotGateway && dotnet run

# Mikroservisler
cd Services/Discount/WebProject.Discount && dotnet run
cd Services/Catalog/WebProject.Catalog && dotnet run
cd Services/Order/Presentation/WebProject.Order.WebApi && dotnet run
cd Services/Cargo/WebProject.Cargo.WebApi && dotnet run
cd Services/Basket/WebProject.Basket && dotnet run
cd Services/Comment/WebProject.Comment && dotnet run
cd Services/Message/WebProject.Message && dotnet run
cd Services/Recommendation/WebProject.Recommendation && dotnet run

# Frontend
cd Frontends/WebProject.WebUI && dotnet run
```

Frontend ayağa kalktıktan sonra tarayıcıdan aç:

```
http://localhost:5083
```

veya (sertifikaya güvendiysen):

```
https://localhost:7177
```

### Sırayla mı, hepsi birden mi?

IdentityServer ve Gateway'in diğerlerinden biraz önce ayakta olması yeterli; sıkı bir başlatma sırası şart değil çünkü her servis kendi bağımlılığına (JWT doğrulama, DB) istek anında bağlanıyor.

## Bilinen sınırlamalar

- **Images.WebUI** servisi gerçek bir Google Cloud Storage bucket + servis hesabı JSON dosyası gerektiriyor (`appsettings.json` içinde placeholder bir Windows yolu var). Bu servis olmadan da ana WebUI çalışır; ürün görselleri için `Image` servisi kullanılıyor.
- `/Information/Index` ve `/AppUser/Profile/Index` yarım kalmış sayfalar (view'ı olmayan ya da model beklerken model almayan controller'lar); uygulamanın hiçbir yerinden linklenmiyorlar.
- **RapidApiWebUI** kendi API anahtarınızı ister:
  ```bash
  cd RapidApi/WebProject.RapidApiWebUI
  dotnet user-secrets init
  dotnet user-secrets set "RapidApi:Key" "kendi-anahtarınız"
  ```

## Docker container'larını durdurma

```bash
docker compose down
```

Veritabanı verisini de silmek isterseniz:

```bash
docker compose down -v
```
