# Canlıya alma (ücretsiz: GCP e2-micro + Neon + MongoDB Atlas)

Uygulama tek bir Docker imajına paketlenir (`deploy/allinone`): 11 .NET servisi + Redis.
Veritabanları dışarıda durur, böylece 1 GB RAM'lik ücretsiz VM'e sığar (~600 MB).

| Parça | Nerede | Not |
|---|---|---|
| Uygulama + Redis | GCP `e2-micro` (Always Free) | `us-west1` / `us-central1` / `us-east1` olmalı |
| PostgreSQL (6 veritabanı) | Neon Free | Ayrı bir proje aç, 5 dk boşta kalınca uyur |
| MongoDB | Atlas M0 Free | Katalog ve öneri verisi |
| İmaj | GitHub Container Registry | GitHub Actions derler |

## 1) İmajı derle (GitHub Actions)

Repo → **Actions → allinone-image → Run workflow** (master'a her push'ta da çalışır).
Bittiğinde Repo → **Packages → webproject-allinone → Package settings → Change visibility → Public**
(sunucu giriş yapmadan çekebilsin diye).

## 2) Neon

[neon.com](https://neon.com) → **New project** (bölge: Frankfurt). Connection details'ten
`host`, `user`, `password`'ü al. Veritabanlarını elle açmana gerek yok, servisler açılışta kendileri oluşturur.

## 3) MongoDB Atlas

[mongodb.com/atlas](https://www.mongodb.com/atlas) → **M0 Free** cluster → Database Access'te kullanıcı aç →
Network Access'e VM'in IP'sini ekle (adım 4'ten sonra) → Connect → Drivers → bağlantı dizesini kopyala.

Örnek katalog verisini bir kez yükle (kendi bilgisayarından, `mongosh` gerekir):

```bash
mongosh "MONGO_URI" scripts/seed_catalog.js
```

## 4) VM

console.cloud.google.com → Compute Engine → **Create instance**:

| Ayar | Değer |
|---|---|
| Region | `us-central1` (veya `us-west1` / `us-east1`) |
| Machine type | `e2-micro` |
| Boot disk | Ubuntu 22.04 LTS, 30 GB **Standard** persistent disk |
| Firewall | ☑ HTTP ☑ HTTPS |

Sonra SSH ile bağlanıp Docker ve swap kur:

```bash
curl -fsSL https://get.docker.com | sudo sh
sudo usermod -aG docker $USER
sudo fallocate -l 2G /swapfile && sudo chmod 600 /swapfile && sudo mkswap /swapfile && sudo swapon /swapfile
echo '/swapfile none swap sw 0 0' | sudo tee -a /etc/fstab
```

Çıkıp tekrar bağlan (docker grubu için).

## 5) Çalıştır

```bash
mkdir webproject && cd webproject
BASE=https://raw.githubusercontent.com/EgeOzdemirr/WebProject/master/deploy/allinone
curl -fsSLO $BASE/docker-compose.yml && curl -fsSLO $BASE/Caddyfile && curl -fsSL $BASE/.env.example -o .env
nano .env        # Neon, Atlas, CLIENT_SECRET, SEED_ADMIN_PASSWORD, DOMAIN, PUBLIC_URL
docker compose up -d
docker compose logs -f app
```

`DOMAIN`: VM'in dış IP'sini tireyle yaz → `34-12-56-78.sslip.io` (ücretsiz, otomatik HTTPS sertifikası alınır).

İlk açılışta IdentityServer demo kullanıcıları oluşturur. Site `https://<DOMAIN>` adresinde açılır.

## Güncelleme

```bash
docker compose pull && docker compose up -d
```
