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

# Sadece csproj dosyasını kopyala → restore hızlı olur
COPY ["KariyerPortali.csproj", "./"]
RUN dotnet restore "KariyerPortali.csproj"

# Tüm proje dosyalarını kopyala
COPY . .

# Publish
RUN dotnet publish "KariyerPortali.csproj" -c Release -o /app/publish -v diag

# ---------------------------
# 3️⃣ Final stage
# ---------------------------
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Container başlatıldığında çalışacak komut
ENTRYPOINT ["dotnet", "KariyerPortali.dll"]
