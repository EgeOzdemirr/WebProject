# WebProject

.NET 6 tabanlı bir e-ticaret mikroservis mimarisi: IdentityServer4 ile kimlik doğrulama, Ocelot API Gateway, ve Catalog/Basket/Order/Discount/Cargo/Comment/Payment/Message/Image/Recommendation gibi bağımsız mikroservisler, hepsi ortak bir MVC frontend (`WebProject.WebUI`) tarafından tüketiliyor.

## Mimari

```
WebProject.WebUI (5083/7177)  ->  Ocelot Gateway (5000)  ->  Mikroservisler (7070-7080)
                               \->  IdentityServer4 (5001)
```

| Servis | Port (http) | Veritabanı |
|---|---|---|
| IdentityServer | 5001 | SQL Server: `WebProjectIdentityDb` |
| Ocelot Gateway | 5000 | - |
| Catalog | 7070 | MongoDB: `WebProjectCatalogDb` + SQL Server: `WebProjectOrderDb` (okuma) |
| Discount | 7071 | SQL Server: `WebProjectDiscountDb` |
| Order | 7072 | SQL Server: `WebProjectOrderDb` |
| Cargo | 7073 | SQL Server: `WebProjectCargoDb` |
| Basket | 7074 | Redis |
| Comment | 7075 | SQL Server: `WebProjectCommentDb` |
| Payment | 7076 | - |
| Image | 7077 | - |
| Message | 7078 | PostgreSQL: `WebProjectMessageDb` |
| Recommendation | 7080 | MongoDB: `WebProjectCatalogDb` |
| RabbitMQMessage, SignalRRealTime, Images.WebUI (GCP), RapidApiWebUI | - | opsiyonel, ana WebUI akışı için gerekli değil |
| **WebUI (frontend)** | **5083 (http) / 7177 (https)** | - |

Tüm mikroservisler `IdentityServerUrl` üzerinden JWT doğrular ve trafik Ocelot Gateway üzerinden `http://localhost:5000/services/<servis>/...` şeklinde yönlenir.

## Gereksinimler

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (SQL Server, MongoDB, PostgreSQL, Redis, RabbitMQ için)

## Kurulum

### 1) Altyapıyı ayağa kaldır

Repo kökünde:

```bash
docker compose up -d
```

Bu, SQL Server (1433), MongoDB (27017), PostgreSQL (5432), Redis (6379) ve RabbitMQ'yu (5672, yönetim paneli 15672) başlatır. SQL Server'ın tam olarak hazır olması ~20-30 saniye sürebilir.

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

Bu `alice` / `Pass123$` ve `bob` / `Pass123$` adında iki test kullanıcısı oluşturur.

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
- **RapidApiWebUI** ve `Admin/ProductController` içinde hardcoded RapidAPI anahtarları var (kaynak kodda açık halde). Bunlar zaten bu repoda public olarak yer alıyor — **RapidAPI hesabınızdan bu anahtarları iptal edip/yenileyip appsettings/user-secrets üzerinden okunacak şekilde taşımanızı öneririz.**
- SQL Server image'ı yalnızca `linux/amd64` için yayınlanıyor; Apple Silicon Mac'lerde Rosetta emülasyonuyla çalışır, ilk açılış birkaç saniye daha uzun sürebilir.

## Docker container'larını durdurma

```bash
docker compose down
```

Veritabanı verisini de silmek isterseniz:

```bash
docker compose down -v
```
