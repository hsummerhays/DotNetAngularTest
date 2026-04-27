# Clean Architecture Analysis for DotNetAngularTest

Based on an analysis of the `DotNetAngularTest` workspace, the backend is explicitly structured to follow **Clean Architecture** (often also called Onion Architecture or Hexagonal Architecture). This architectural style is designed to create systems that are independent of frameworks, testable, independent of the UI, independent of the database, and independent of any external agency.

Here is a breakdown of how the project adheres to the core principles of Clean Architecture:

## 1. The Dependency Rule
The overriding rule that makes Clean Architecture work is **The Dependency Rule**: *Source code dependencies must point only inward, toward higher-level policies.*

**How it follows:**
* **`AwsApp.Domain`** has zero dependencies on any other project in the solution. It sits at the very center of the architecture.
* **`AwsApp.Application`** depends only on `AwsApp.Domain`. It knows nothing about SQL Server, AWS S3, or HTTP requests.
* **`AwsApp.Infrastructure`** depends on `AwsApp.Application` (and transitively `AwsApp.Domain`) to implement the interfaces defined in the Application layer.
* **`AwsApp.API`** depends on `AwsApp.Application` and `AwsApp.Infrastructure` (solely for Dependency Injection wiring at startup).

## 2. Entities (Domain Layer)
This layer encapsulates the enterprise-wide business rules.

**How it follows:**
* The `AwsApp.Domain` project holds the core entities, value objects, domain exceptions, and domain events. By isolating this from everything else, the core business logic remains completely unaffected by changes to external frameworks (like upgrading Entity Framework or changing the web framework).

## 3. Use Cases (Application Layer)
The software in this layer contains application-specific business rules. It encapsulates and implements all the use cases of the system.

**How it follows:**
* The `AwsApp.Application` project utilizes the CQRS pattern via MediatR (e.g., `GetProductsQuery` and `GetProductsQueryHandler`). Each handler represents a distinct use case.
* It orchestrates the flow of data to and from the entities, and directs those entities to use their enterprise-wide business rules to achieve the goals of the use case.
* It defines interfaces (like `IFileService` and `IApplicationDbContext`) that it needs to do its job, but it does *not* implement them. This is the application of the Dependency Inversion Principle that makes Clean Architecture possible.

## 4. Interface Adapters (Infrastructure & API Layers)
The software in this layer is a set of adapters that convert data from the format most convenient for the use cases and entities, to the format most convenient for some external agency such as the Database or the Web.

**How it follows:**
* **`AwsApp.Infrastructure`:** Contains classes like `S3FileService` and `ApplicationDbContext`. These classes adapt the generic Application layer interfaces to specific technologies (AWS S3 SDK, Entity Framework Core).
* **`AwsApp.API`:** The controllers (like `ProductsController`) act as adapters that take HTTP requests from the outside world, convert them into MediatR Queries/Commands, and pass them inward to the Application layer.

## 5. Independence of UI, Database, and Frameworks
**How it follows:**
* **Database:** The database is abstracted behind `IApplicationDbContext`. You could swap SQL Server for PostgreSQL or a mock in-memory database for testing without changing a single line of code in the Application or Domain layers.
* **External Services:** AWS S3 logic is kept strictly inside the Infrastructure layer. The Application layer just says "Upload this file" via `IFileService`.
* **UI:** The Angular frontend is completely decoupled. The API serves JSON, and the Angular app consumes it. The API doesn't know or care if the UI is an Angular web app, a mobile app, or a CLI.

---

## Potential Areas to Watch Out For

While the structure perfectly aligns with Clean Architecture, here are common pitfalls that developers can sometimes fall into when working within this pattern:

1. **Domain Logic Leakage:** Placing core business rules into the Application layer (the MediatR handlers) instead of the Domain entities. Entities should be rich and encapsulate their own behavior, not just act as anemic data bags.
2. **Infrastructure Leakage:** Returning Entity Framework tracking proxies directly to the Application layer, or using infrastructure-specific types (like `Microsoft.AspNetCore.Http.IFormFile`) inside the `AwsApp.Application` project. The Application layer should only use primitive types or Domain/Application specific DTOs.
3. **Over-Engineering:** For very simple CRUD operations, the ceremony of creating a Command, a CommandHandler, a Validator, and mapping to a DTO can feel like overkill. However, for a growing enterprise app, this structure pays massive dividends in maintainability.
