# Item Comparison API (C# .NET 8)

API REST que permite **listar productos**, **buscar por término** y **comparar por especificaciones**.  
Los datos se leen desde un archivo JSON (`products.json`), sin necesidad de base de datos real.

---

## Requisitos

- [SDK .NET 8.0+](https://dotnet.microsoft.com/download)
- Git (opcional)
- No requiere base de datos

---

## Cómo levantar la API

### Windows (PowerShell)
```
dotnet restore
dotnet build
dotnet run --project src\ItemComparison.Api
```
🔗 Swagger UI: http://localhost:5102/swagger

🔗 Base URL: http://localhost:5102

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

## Estructura del proyecto
```
ItemComparisonMELI/
├─ src/
│ └─ ItemComparison.Api/ # Proyecto principal de la API
│ ├─ Controllers/
│ │ └─ ProductsController.cs
│ ├─ Data/
│ │ ├─ IProductRepository.cs
│ │ └─ InMemoryProductRepository.cs
│ ├─ Dtos/
│ │ ├─ CompareDtos.cs
│ │ └─ ProductDto.cs
│ ├─ Middleware/
│ │ └─ ErrorHandlingMiddleware.cs
│ ├─ Models/
│ │ ├─ Product.cs
│ │ └─ Specification.cs
│ ├─ Validators/
│ │ └─ CompareRequestValidator.cs
│ ├─ Properties/
│ │ └─ launchSettings.json
│ ├─ appsettings.Development.json
│ ├─ appsettings.json
│ ├─ products.json # Datos mockeados
│ ├─ Program.cs # Entrada principal
│ └─ ItemComparison.Api.csproj
├─ tests/
│ └─ ItemComparison.Tests/ # Proyecto de tests
│ ├─ RepositoryTests.cs
│ ├─ UnitTest1.cs
│ └─ ItemComparison.Tests.csproj
├─ .gitignore
├─ ItemComparisonApi.sln # Solución
├─ README.md # Documentación
└─ prompts.md # Prompts usados con IA
```
