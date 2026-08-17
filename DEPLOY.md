# Canlıya alma (Oracle Cloud Always Free)

Bu doküman, VM hazır olduğunda sunucu üzerinde çalıştırılacak adımları anlatır.

## 1) Sunucuya bağlan ve Docker kur

```bash
ssh -i <indirdiğin-private-key> ubuntu@<VM_PUBLIC_IP>

sudo apt-get update
sudo apt-get install -y ca-certificates curl gnupg
sudo install -m 0755 -d /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu $(. /etc/os-release && echo "$VERSION_CODENAME") stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
sudo apt-get update
sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin
sudo usermod -aG docker $USER
# çıkış yapıp tekrar ssh ile bağlan (grup değişikliğinin etkili olması için)
```

## 2) Oracle Cloud güvenlik listesinde 80 (ve istersen 443) portunu aç

Oracle Cloud Console → Networking → Virtual Cloud Networks → (VCN'in) → Security Lists → Default Security List → Add Ingress Rules:
- Source CIDR: `0.0.0.0/0`, IP Protocol: TCP, Destination Port: `80`
- (opsiyonel, ileride domain+HTTPS için) Destination Port: `443`

Sunucunun kendi güvenlik duvarı da aynı portu açık tutmalı:
```bash
sudo iptables -I INPUT -p tcp --dport 80 -j ACCEPT
sudo netfilter-persistent save 2>/dev/null || true
```

## 3) Repoyu çek ve ayağa kaldır

```bash
git clone https://github.com/EgeOzdemirr/WebProject.git
cd WebProject
docker compose -f docker-compose.prod.yml up -d --build
```

İlk build 10-20 dakika sürebilir (11 .NET servisi + SQL Server image indirme). İlerlemeyi izlemek için:

```bash
docker compose -f docker-compose.prod.yml logs -f
```

## 4) IdentityServer'ı seed et (tek seferlik)

Tüm container'lar ayağa kalktıktan (özellikle `sqlserver` sağlıklı olduktan) sonra:

```bash
docker compose -f docker-compose.prod.yml run --rm identityserver dotnet WebProject.IdentityServer.dll /seed
```

Bu `bob` (Admin) ve `alice` (normal kullanıcı) hesaplarını oluşturur — bkz. [README.md](README.md).

## 5) Örnek katalog verisini yükle (opsiyonel, sadece demo için)

```bash
docker cp scripts/seed_catalog.js $(docker compose -f docker-compose.prod.yml ps -q mongodb):/tmp/seed_catalog.js
docker compose -f docker-compose.prod.yml exec mongodb mongosh --quiet /tmp/seed_catalog.js
```

## 6) Test et

Tarayıcıdan `http://<VM_PUBLIC_IP>` adresine git.

## Notlar / sonraki adımlar

- Şu an site düz HTTP üzerinden yayında (henüz domain/HTTPS yok). Bir domain alıp VM'in IP'sine yönlendirdikten sonra, önüne Caddy/nginx ile ücretsiz Let's Encrypt HTTPS eklemek kolay bir sonraki adım.
- `docker-compose.prod.yml` içindeki `AllowedOrigins__0` değerini gerçek public URL ile güncellemek gerekir (şu an placeholder).
- Bu ilk sürümde Payment, Image, RabbitMQMessage, SignalRRealTime, Images.WebUI ve RapidApiWebUI servisleri dahil edilmedi — ana WebUI akışı bunlara ihtiyaç duymuyor (bkz. README). İstenirse ayrıca eklenebilir.
- Güncelleme yapmak için: `git pull && docker compose -f docker-compose.prod.yml up -d --build`
