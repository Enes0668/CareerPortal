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

# Önce csproj dosyasını kopyala → restore hızlı olur
COPY ["KariyerPortalı/KariyerPortalı.csproj", "KariyerPortalı/"]
RUN dotnet restore "KariyerPortalı/KariyerPortalı.csproj"

# Tüm proje dosyalarını kopyala
COPY . .
WORKDIR "/src/KariyerPortalı"

# Build ve publish
RUN dotnet publish "KariyerPortalı.csproj" -c Release -o /app/publish

# ---------------------------
# 3️⃣ Final stage
# ---------------------------
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Container başlatıldığında çalışacak komut
ENTRYPOINT ["dotnet", "KariyerPortalı.dll"]
