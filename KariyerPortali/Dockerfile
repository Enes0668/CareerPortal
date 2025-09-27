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

# sadece csproj dosyasını kopyala → restore hızlı olur
COPY ["KariyerPortali/KariyerPortali.csproj", "KariyerPortali/"]

WORKDIR /src/KariyerPortali
RUN dotnet restore "KariyerPortali.csproj"

# tüm proje dosyalarını kopyala
COPY . .

# publish
RUN dotnet publish "KariyerPortali.csproj" -c Release -o /app/publish

# ---------------------------
# 3️⃣ Final stage
# ---------------------------
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# container başlatıldığında çalışacak komut
ENTRYPOINT ["dotnet", "KariyerPortali.dll"]
