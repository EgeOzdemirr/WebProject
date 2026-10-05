#!/bin/bash
# Tüm süreçleri başlatır. Biri düşerse container kapanır (Render yeniden başlatır).
set -u

: "${PORT:=10000}"
: "${DB_HOST:?DB_HOST tanımlı olmalı}"
: "${DB_USER:?DB_USER tanımlı olmalı}"
: "${DB_PASSWORD:?DB_PASSWORD tanımlı olmalı}"
: "${CLIENT_SECRET:?CLIENT_SECRET tanımlı olmalı}"
: "${MONGO_URI:?MONGO_URI tanımlı olmalı}"
DB_PORT="${DB_PORT:-5432}"
# Neon için:  DB_EXTRA=";SSL Mode=Require;Trust Server Certificate=true;Timeout=30"
DB_EXTRA="${DB_EXTRA:-}"
PUBLIC_URL="${PUBLIC_URL:-http://localhost:$PORT}"

pg() { echo "Server=${DB_HOST};Port=${DB_PORT};Database=$1;User Id=${DB_USER};Password=${DB_PASSWORD}${DB_EXTRA}"; }

# Kalıcı anahtarlar (Render ücretsiz katmanda disk yok): ortam değişkeninden dosyaya yaz
if [ -n "${IDS_SIGNING_KEY_JWK:-}" ]; then echo "$IDS_SIGNING_KEY_JWK" > /keys/tempkey.jwk; fi
if [ -n "${DP_KEY_XML:-}" ]; then echo "$DP_KEY_XML" > /dpkeys/key-static.xml; fi

echo "[start] redis"
redis-server --port 6379 --bind 127.0.0.1 --save "" --appendonly no --maxmemory 24mb --maxmemory-policy allkeys-lru --loglevel warning &

ID_URL="http://127.0.0.1:5001"
# 512 MB RAM'e sığmak için her süreç için yönetilen yığın sınırı ve agresif çöp toplama
export DOTNET_GCHeapHardLimit="${DOTNET_GCHeapHardLimit:-0x3000000}"   # 48 MB
export DOTNET_GCConserveMemory="${DOTNET_GCConserveMemory:-9}"
export DOTNET_gcConcurrent=0
start() { # ad  dll  port  [ENV=değer ...]
  local dir="$1" dll="$2" port="$3"; shift 3
  ( cd "/app/$dir" && exec env ASPNETCORE_URLS="http://127.0.0.1:$port" ASPNETCORE_ENVIRONMENT=Production IdentityServerUrl="$ID_URL" "$@" dotnet "$dll" ) 2>&1 | sed -u "s/^/[$dir] /" &
}

# IdentityServer: önce veritabanı + örnek kullanıcılar (tekrar çalıştırılması zararsız)
echo "[start] identityserver seed"
( cd /app/WebProject.IdentityServer && \
  CLIENT_SECRET="$CLIENT_SECRET" ConnectionStrings__DefaultConnection="$(pg WebProjectIdentityDb)" \
  dotnet WebProject.IdentityServer.dll /seed ) 2>&1 | sed -u 's/^/[seed] /'

start WebProject.IdentityServer WebProject.IdentityServer.dll 5001 \
  CLIENT_SECRET="$CLIENT_SECRET" SigningKeyPath=/keys/tempkey.jwk \
  ConnectionStrings__DefaultConnection="$(pg WebProjectIdentityDb)"

start WebProject.OcelotGateway WebProject.OcelotGateway.dll 5000 ASPNETCORE_ENVIRONMENT=AllInOne

start WebProject.Discount WebProject.Discount.dll 7071 ConnectionStrings__DefaultConnection="$(pg WebProjectDiscountDb)"
start WebProject.Order.WebApi WebProject.Order.WebApi.dll 7072 ConnectionStrings__DefaultConnection="$(pg WebProjectOrderDb)"
start WebProject.Cargo.WebApi WebProject.Cargo.WebApi.dll 7073 ConnectionStrings__DefaultConnection="$(pg WebProjectCargoDb)"
start WebProject.Comment WebProject.Comment.dll 7075 ConnectionStrings__DefaultConnection="$(pg WebProjectCommentDb)"
start WebProject.Message WebProject.Message.dll 7078 ConnectionStrings__DefaultConnection="$(pg WebProjectMessageDb)"
start WebProject.Catalog WebProject.Catalog.dll 7070 \
  ConnectionStrings__OrderingDb="$(pg WebProjectOrderDb)" ConnectionStrings__MongoDB="$MONGO_URI" DatabaseSettings__ConnectionString="$MONGO_URI"
start WebProject.Recommendation WebProject.Recommendation.dll 7080 MongoDb__ConnectionString="$MONGO_URI"
start WebProject.Basket WebProject.Basket.dll 7074 RedisSettings__Host=127.0.0.1 RedisSettings__Port=6379

# WebUI dışarıya açılan tek süreç
( cd /app/WebProject.WebUI && exec env ASPNETCORE_URLS="http://0.0.0.0:$PORT" ASPNETCORE_ENVIRONMENT=Production \
    ServiceApiSettings__OcelotUrl="http://127.0.0.1:5000" ServiceApiSettings__IdentityServerUrl="$ID_URL" \
    ClientSettings__WebProjectVisitorClient__ClientSecret="$CLIENT_SECRET" \
    ClientSettings__WebProjectManagerClient__ClientSecret="$CLIENT_SECRET" \
    ClientSettings__WebProjectAdminClient__ClientSecret="$CLIENT_SECRET" \
    DataProtectionKeysPath=/dpkeys AllowedOrigins__0="$PUBLIC_URL" \
    dotnet WebProject.WebUI.dll ) 2>&1 | sed -u 's/^/[WebProject.WebUI] /' &

echo "[start] tüm süreçler başlatıldı, WebUI :$PORT"
wait -n
echo "[start] bir süreç sonlandı, container kapanıyor" >&2
exit 1
