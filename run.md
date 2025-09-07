## Cómo levantar la API

### Windows (PowerShell)
```
dotnet restore
dotnet build
dotnet run --project src\ItemComparison.Api
```
🔗 Swagger UI: http://localhost:5102/swagger

🔗 Base URL: http://localhost:5102

## Test
```
dotnet test

```

## Listar productos (página 1, 5 resultados, búsqueda "ram")
```
curl "http://localhost:5102/api/products?q=ram&page=1&pageSize=5"
```
## Detalle
```
curl "http://localhost:5102/api/products/p-iphone15"
```
## Comparar
```
curl -X POST "http://localhost:5102/api/products/compare" \
  -H "Content-Type: application/json" \
  -d "{\"productIds\":[\"p-iphone15\",\"p-pixel8\"]}"
```

## Health
```
curl "http://localhost:5102/health"
```
## Manejo de errores

400 Bad Request → parámetros inválidos o request mal formado

404 Not Found → producto no encontrado

500 Internal Server Error → error inesperado
