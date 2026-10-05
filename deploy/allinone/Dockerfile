# syntax=docker/dockerfile:1
# Tüm servisleri (WebUI, Gateway, IdentityServer + 8 mikroservis) ve Redis'i TEK container'da çalıştırır.
# Tek bir sunucuya/servise (ör. küçük bir VM ya da Render) paketlemek için: tüm .NET servislerini TEK container
# bu şekilde paketlenir. Dışarıya sadece WebUI ($PORT) açılır, diğerleri 127.0.0.1'de dinler.
# Derleme (repo kökünden):  docker build -f deploy/allinone/Dockerfile -t webproject-allinone .
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN --mount=type=cache,target=/root/.nuget/packages set -e; \
    for entry in \
      "IdentityServer/WebProject.IdentityServer/WebProject.IdentityServer.csproj" \
      "ApiGateway/WebProject.OcelotGateway/WebProject.OcelotGateway.csproj" \
      "Services/Catalog/WebProject.Catalog/WebProject.Catalog.csproj" \
      "Services/Discount/WebProject.Discount/WebProject.Discount.csproj" \
      "Services/Order/Presentation/WebProject.Order.WebApi/WebProject.Order.WebApi.csproj" \
      "Services/Cargo/WebProject.Cargo.WebApi/WebProject.Cargo.WebApi.csproj" \
      "Services/Basket/WebProject.Basket/WebProject.Basket.csproj" \
      "Services/Comment/WebProject.Comment/WebProject.Comment.csproj" \
      "Services/Message/WebProject.Message/WebProject.Message.csproj" \
      "Services/Recommendation/WebProject.Recommendation/WebProject.Recommendation.csproj" \
      "Frontends/WebProject.WebUI/WebProject.WebUI.csproj"; do \
        name=$(basename "$entry" .csproj); \
        dotnet publish "$entry" -c Release -o "/out/$name"; \
    done

# Redis (Basket servisi için), aspnet:6.0 ile aynı Debian 11 olan resmi imajdan alınır
FROM redis:6.2-bullseye AS redis

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
COPY --from=redis /usr/local/bin/redis-server /usr/local/bin/redis-server
WORKDIR /app
COPY --from=build /out /app
COPY deploy/allinone/start.sh /app/start.sh
RUN chmod +x /app/start.sh && mkdir -p /keys /dpkeys
# Küçük bellek için .NET ayarları
ENV DOTNET_gcServer=0 \
    DOTNET_GCgen0size=0x400000 \
    DOTNET_TieredPGO=0 \
    DOTNET_TC_QuickJitForLoops=1
EXPOSE 10000
CMD ["/app/start.sh"]
