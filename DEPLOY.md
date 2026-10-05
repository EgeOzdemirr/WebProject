# Canlıya alma (Google Cloud — ücretsiz katman)

Bu doküman projeyi kalıcı ve ücretsiz bir sunucuda yayına almayı anlatır.

Google Cloud'un "Always Free" katmanı, süresiz ücretsiz bir `e2-micro` VM
veriyor. Stack'in bellek kullanımı ~550 MB olduğu için bu makineye sığıyor
(SQL Server kaldırılıp PostgreSQL'e geçildiği için — bkz. README).

> Ücretsiz olması için VM **`us-west1`, `us-central1` veya `us-east1`**
> bölgelerinden birinde ve `e2-micro` tipinde olmalı. Başka bölge/tip
> seçilirse normal ücretlendirme işler.

## 1) VM oluştur

[console.cloud.google.com](https://console.cloud.google.com) → Compute Engine
→ VM instances → **Create instance**:

| Ayar | Değer |
|---|---|
| Region | `us-central1` (veya `us-west1` / `us-east1`) |
| Machine type | `e2-micro` |
| Boot disk | Ubuntu 22.04 LTS, 30 GB **Standard persistent disk** |
| Firewall | ☑ Allow HTTP traffic |

30 GB standard disk de ücretsiz katmana dahil; SSD seçilirse ücretlendirilir.

`e2-micro` 1 GB RAM'e sahip. İlk `docker build` sırasında bellek yetmeyebilir,
bu yüzden aşağıda swap ekliyoruz.

## 2) Sunucuya bağlan ve Docker kur

Konsoldaki **SSH** düğmesiyle bağlan, sonra:

```bash
sudo apt-get update
sudo apt-get install -y ca-certificates curl gnupg
sudo install -m 0755 -d /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu $(. /etc/os-release && echo "$VERSION_CODENAME") stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
sudo apt-get update
sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin
sudo usermod -aG docker $USER
```

Grup değişikliğinin geçerli olması için çıkıp tekrar bağlan.

## 3) Swap ekle (1 GB RAM için gerekli)

Build sırasında derleyici geçici olarak çok bellek ister:

```bash
sudo fallocate -l 2G /swapfile
sudo chmod 600 /swapfile
sudo mkswap /swapfile
sudo swapon /swapfile
echo '/swapfile none swap sw 0 0' | sudo tee -a /etc/fstab
```

## 4) Repoyu çek ve ayağa kaldır

```bash
git clone https://github.com/EgeOzdemirr/WebProject.git
cd WebProject
cp .env.example .env
# .env içindeki POSTGRES_PASSWORD, CLIENT_SECRET, SEED_ADMIN_PASSWORD'ü doldur (openssl rand -base64 24)
sed -i "s|^PUBLIC_URL=.*|PUBLIC_URL=http://$(curl -s ifconfig.me)|" .env
docker compose -f docker-compose.prod.yml up -d --build
```

İlk build `e2-micro` üzerinde 30-50 dakika sürebilir (tek çekirdek). İzlemek için:

```bash
docker compose -f docker-compose.prod.yml logs -f
```

> Build çok yavaş gelirse alternatif: image'ları kendi bilgisayarında
> `--platform linux/amd64` ile build edip bir registry'ye (Docker Hub /
> GitHub Container Registry) push et, sunucuda sadece `pull` et.

## 5) IdentityServer'ı seed et (tek seferlik)

Tüm container'lar ayağa kalktıktan sonra:

```bash
docker compose -f docker-compose.prod.yml run --rm identityserver dotnet WebProject.IdentityServer.dll /seed
```

Bu `bob` (Admin, parolası `.env`'deki `SEED_ADMIN_PASSWORD`; boşsa konsola bir kez yazılır) ve `alice` (herkese açık demo kullanıcısı) hesaplarını oluşturur.

## 6) Örnek katalog verisini yükle (demo için)

```bash
docker cp scripts/seed_catalog.js $(docker compose -f docker-compose.prod.yml ps -q mongodb):/tmp/seed_catalog.js
docker compose -f docker-compose.prod.yml exec mongodb mongosh --quiet /tmp/seed_catalog.js
```

## 7) Test et ve README'ye linki koy

Tarayıcıdan `http://<VM_EXTERNAL_IP>` adresine git. Çalıştığını gördükten
sonra README'deki canlı demo satırını güncelle:

```bash
sed -i "s|<!-- LIVE_URL -->.*<!-- /LIVE_URL -->|<!-- LIVE_URL -->http://<VM_EXTERNAL_IP><!-- /LIVE_URL -->|" README.md
```

VM'in IP'sinin sabit kalması için Console → VPC network → IP addresses
üzerinden external IP'yi **Static** yapmak gerekir (aksi halde VM yeniden
başlatılınca IP değişir). Kullanımdaki statik IP ücretsiz katmana dahildir.

## Notlar / sonraki adımlar

- Site düz HTTP üzerinden yayında. Bir domain alıp VM'in IP'sine
  yönlendirdikten sonra önüne Caddy koyup ücretsiz Let's Encrypt HTTPS
  eklemek kolay bir sonraki adım.
- `.env` içindeki `PUBLIC_URL` WebUI'ın CORS ayarını besliyor; domain
  aldığında bu değeri güncelleyip `up -d` ile yeniden başlat.
- Güncelleme: `git pull && docker compose -f docker-compose.prod.yml up -d --build`
- Bellek durumunu izlemek için: `docker stats --no-stream`
