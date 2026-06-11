# VyapSetuAPI — Shop Inventory & Billing System

Production-ready ASP.NET Core 8 Web API for shop inventory management and customer billing, built with Clean Architecture.

## Solution Structure

```
VyapSetuAPI/
├── VyapSetuAPI.sln
├── docs/
│   └── angular-api-endpoints.ts
├── ShopManagementSystem.API/           # Presentation layer (Controllers, Middleware)
├── ShopManagementSystem.Application/   # Application layer (Services, DTOs, Validators)
├── ShopManagementSystem.Domain/        # Domain layer (Entities, Interfaces)
└── ShopManagementSystem.Infrastructure/# Infrastructure layer (EF Core, Repositories)
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or SQL Server LocalDB (included with Visual Studio)
- [Git](https://git-scm.com/downloads)
- Optional: [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

## Technology Stack

| Component | Technology |
|-----------|------------|
| Framework | ASP.NET Core 8 Web API |
| ORM | Entity Framework Core 8 |
| Database | SQL Server |
| Mapping | AutoMapper |
| Validation | FluentValidation |
| Documentation | Swagger/OpenAPI |
| Patterns | Repository, Unit of Work, Service Layer, DI |

## Database Setup

1. Update the connection string in `ShopManagementSystem.API/appsettings.json` if needed:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=VyapSetuDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

For a full SQL Server instance, use:

```
Server=localhost;Database=VyapSetuDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true
```

2. Apply migrations (also runs automatically on startup via seed):

```bash
dotnet ef database update --project ShopManagementSystem.Infrastructure --startup-project ShopManagementSystem.API
```

## Migration Commands

```bash
# Add a new migration
dotnet ef migrations add MigrationName --project ShopManagementSystem.Infrastructure --startup-project ShopManagementSystem.API --output-dir Persistence/Migrations

# Apply migrations to database
dotnet ef database update --project ShopManagementSystem.Infrastructure --startup-project ShopManagementSystem.API

# Remove last migration (if not applied)
dotnet ef migrations remove --project ShopManagementSystem.Infrastructure --startup-project ShopManagementSystem.API
```

## Run Instructions

```bash
# Restore and build
dotnet restore VyapSetuAPI.sln
dotnet build VyapSetuAPI.sln

# Run the API
dotnet run --project ShopManagementSystem.API
```

- Swagger UI: [http://localhost:5000](http://localhost:5000) (Development)
- HTTPS: [https://localhost:7050](https://localhost:7050)

Seed data includes 3 products and 2 customers on first run.

## API Response Format

All endpoints return a consistent wrapper:

```json
{
  "success": true,
  "message": "Optional message",
  "data": { },
  "errors": null
}
```

## API Endpoints

### Products

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get product by ID |
| GET | `/api/products/search?term={term}` | Search by name or product ID |
| POST | `/api/products` | Create product |
| PUT | `/api/products/{id}` | Update product |
| DELETE | `/api/products/{id}` | Delete product |

**Create Product Request:**
```json
{
  "productId": "PRD-004",
  "productName": "HDMI Cable",
  "costPrice": 299.00,
  "gst": 18.00,
  "quantity": 75
}
```

### Customers

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/customers` | Get all customers |
| GET | `/api/customers/{id}` | Get customer by ID |
| GET | `/api/customers/phone/{phoneNumber}` | Get customer by phone |
| POST | `/api/customers` | Create customer |
| PUT | `/api/customers/{phoneNumber}` | Update customer |
| DELETE | `/api/customers/{phoneNumber}` | Delete customer |

**Create Customer Request:**
```json
{
  "customerName": "Amit Kumar",
  "phoneNumber": "9988776655",
  "address": "78 Ring Road, Delhi"
}
```

## Business Rules

- `ProductId` must be unique across products
- `PhoneNumber` must be unique across customers (10 digits)
- Stock is validated before billing; insufficient stock returns HTTP 400
- Inventory is deducted after successful bill creation (transactional)
- `BillNumber` is auto-generated: `BILL-YYYYMMDD-0001`
- GST, SubTotal, and GrandTotal are calculated automatically

## Angular Integration

See `docs/angular-api-endpoints.ts` for ready-to-use endpoint constants and an example Angular service.

Configure CORS in `Program.cs` for your Angular dev server (default: `http://localhost:4200`).

**environment.ts example:**
```typescript
export const environment = {
  production: false,
  apiBaseUrl: 'http://localhost:5000'
};
```

## Push to GitHub

```bash
# Initialize repository (already done in this project)
git init

# Stage and commit
git add .
git commit -m "Initial commit: VyapSetu Shop Management API"

# Create repository on GitHub, then:
git remote add origin https://github.com/YOUR_USERNAME/VyapSetuAPI.git
git branch -M main
git push -u origin main
```

### Step-by-Step GitHub Push

1. Create a new repository on [GitHub](https://github.com/new) named `VyapSetuAPI` (do not initialize with README).
2. Open a terminal in the project root (`VYAP`).
3. Run:
   ```bash
   git remote add origin https://github.com/YOUR_USERNAME/VyapSetuAPI.git
   git branch -M main
   git push -u origin main
   ```
4. Verify the repository on GitHub contains all projects and the README.

## License

MIT
