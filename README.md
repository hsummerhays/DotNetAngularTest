# Clean Architecture .NET & Angular AWS App

Successfully scaffolded a modern application structure following Clean Architecture principles, optimized for AWS deployment.

## Project Structure

```bash
├── src
│   ├── AwsApp.API            # Presentation Layer: ASP.NET Core Web API
│   ├── AwsApp.Application    # Application Layer: Business Logic & MediatR
│   ├── AwsApp.Domain         # Domain Layer: Entities & Domain Logic
│   ├── AwsApp.Infrastructure # Infrastructure Layer: Persistence & AWS Services
│   └── frontend              # Frontend Layer: Angular 20 LTS Application
├── terraform                # IaC: Infrastructure as Code for AWS
└── AwsApp.slnx
```

## Layers
- **Domain**: Contains entities like `Product` and base classes. No dependencies on other layers.
- **Application**: Business logic handling via MediatR and core interfaces.
- **Infrastructure**: Persistence (EF Core) and AWS Integrations (S3, etc.).
- **API**: Presentation layer, Scalar API documentation, and Controller endpoints.

## Tech Stack
- **Backend**: .NET 10 (LTS), C# 14
- **API Reference**: Scalar (Modern OpenAPI UI)
- **Frontend**: Angular 20 (LTS), TypeScript 5.8
- **Cloud**: AWS (App Runner, S3, RDS)
- **IaC**: Terraform

## Running Locally

### The Fast Way (Recommended)
Run the automated startup script to launch both the backend and frontend in a split terminal window:

```powershell
.\run.ps1
```

### Manual Startup
1. **Backend**: 
   ```bash
   cd src/AwsApp.API
   dotnet run
   ```
2. **Frontend**:
   ```bash
   cd src/frontend
   npm start
   ```

## Development Endpoints
- **Frontend**: [http://localhost:4200](http://localhost:4200)
- **API Reference (Scalar)**: [http://localhost:5067/scalar/v1](http://localhost:5067/scalar/v1)
