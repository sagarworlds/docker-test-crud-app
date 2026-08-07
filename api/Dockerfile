# ── Build Stage ────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY CrudApp.Api.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish

# ── Runtime Stage ───────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Disable inotify file reload watcher to prevent cloud container crash (inotify instance limit)
ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "CrudApp.Api.dll"]
