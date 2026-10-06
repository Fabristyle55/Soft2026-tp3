# syntax=docker/dockerfile:1

# ---------- Etapa 1: build ----------
# Imagen con el SDK completo: restaura paquetes, compila y publica.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos primero solo los .csproj para aprovechar la caché de capas:
# si no cambian las dependencias, "dotnet restore" no se vuelve a ejecutar.
COPY Dsw2025Tpi.Api/Dsw2025Tpi.Api.csproj                 Dsw2025Tpi.Api/
COPY Dsw2025Tpi.Application/Dsw2025Tpi.Application.csproj Dsw2025Tpi.Application/
COPY Dsw2025Tpi.Data/Dsw2025Tpi.Data.csproj               Dsw2025Tpi.Data/
COPY Dsw2025Tpi.Domain/Dsw2025Tpi.Domain.csproj           Dsw2025Tpi.Domain/
RUN dotnet restore Dsw2025Tpi.Api/Dsw2025Tpi.Api.csproj

# Ahora sí, el resto del código fuente.
COPY . .
RUN dotnet publish Dsw2025Tpi.Api/Dsw2025Tpi.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

# ---------- Etapa 2: runtime ----------
# Imagen liviana: solo el runtime de ASP.NET Core, sin SDK ni código fuente.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Valores por defecto; se sobreescriben con -e / docker-compose / App Settings de Azure.
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_HTTP_PORTS=8080 \
    DOTNET_RUNNING_IN_CONTAINER=true
EXPOSE 8080

COPY --from=build /app/publish .

# Usuario sin privilegios que trae la imagen oficial (.NET 8+).
USER $APP_UID

ENTRYPOINT ["dotnet", "Dsw2025Tpi.Api.dll"]
