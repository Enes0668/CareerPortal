# ---------------------------
# 1️⃣ Base image (runtime)
# ---------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# ---------------------------
# 2️⃣ Build image
# ---------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Projenin tüm içeriğini kopyala (alt klasör karmaşasını önlemek için)
COPY KariyerPortali/ ./KariyerPortali/

WORKDIR /src/KariyerPortali

# Restore
RUN dotnet restore

# Publish
RUN dotnet publish -c Release -o /app/publish

# ---------------------------
# 3️⃣ Final stage
# ---------------------------
FROM base AS final
WORKDIR /app

# Build aşamasındaki publish dizinini final stage'e kopyala
COPY --from=build /app/publish .

# Container başladığında çalışacak komut
ENTRYPOINT ["dotnet", "KariyerPortali.dll"]
