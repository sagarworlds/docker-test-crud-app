# ── Build Stage ────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file from api directory
COPY api/*.csproj ./api/
RUN dotnet restore api/CrudApp.Api.csproj

# Copy source code and build
COPY api/ ./api/
RUN dotnet publish api/CrudApp.Api.csproj -c Release -o /app/publish

# ── Runtime Stage ───────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Disable inotify file reload watcher to prevent cloud container crash
ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "CrudApp.Api.dll"]
