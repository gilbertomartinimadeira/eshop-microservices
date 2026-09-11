# Local development

Run PostgreSQL in Docker:

```bash
docker compose -f docker-compose.development.yml up -d
```

Run the Catalog API locally with the Development configuration:

```bash
dotnet run --project src/Services/Catalog.API/Catalog.API.csproj
```

The API listens on `http://localhost:5000` and connects to PostgreSQL on
`localhost:5432` using the connection string in
`src/Services/Catalog.API/appsettings.Development.json`.

Stop the development database with:

```bash
docker compose -f docker-compose.development.yml down
```