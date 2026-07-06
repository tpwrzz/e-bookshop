# BookShop

An e-bookshop platform built with .NET 10 to learn and apply production microservices patterns — Clean Architecture, DDD, CQRS, event-driven messaging, and distributed transactions.

Each service is independently deployable, owns its own database, and communicates asynchronously via RabbitMQ. No service shares a schema or references another service's code directly.

## Services

- **Catalog** — books, authors, genres, search (MSSQL)
- **Ordering** — order lifecycle with a full DDD aggregate (MSSQL)
- **Basket** — per-user shopping cart (Couchbase)
- **Payments** — payment processing, choreography Saga (MSSQL)
- **Notifications** — event-driven email/push consumer (DynamoDB)
- **Identity** — auth and JWT (MSSQL)

## Stack

.NET 10, ASP.NET Core, Entity Framework Core, MediatR, FluentValidation, MassTransit, Serilog, RabbitMQ, Kafka, Couchbase, MSSQL, DynamoDB, YARP, Docker, Kubernetes, Azure, Grafana, DataDog.

## Patterns

- Clean Architecture enforced at the project level — Domain has no outward dependencies
- DDD aggregates with domain events, value objects, and status transition guards
- CQRS via MediatR with a ValidationBehavior and LoggingBehavior pipeline
- Outbox pattern for reliable domain event publishing — order and outbox message written in one transaction
- Choreography Saga across Ordering and Payments with idempotency guards on every consumer
- Database-per-service — no shared schemas, no cross-service joins
- Result<T> pattern throughout — no exceptions for expected failures

## Running locally

```bash
git clone https://github.com/your-username/e-bookshop.git
cd e-bookshop/e-bookshop
docker compose up -d
```

Apply migrations:

```bash
dotnet ef database update --project Catalog/Catalog.Infrastructure --startup-project Catalog/Catalog.API
dotnet ef database update --project Ordering/Ordering.Infrastructure --startup-project Ordering/Ordering.API
dotnet ef database update --project Payments/Payments.Infrastructure --startup-project Payments/Payments.API
```

Swagger at `localhost:5103` (Catalog), `localhost:5263` (Ordering), `localhost:5189` (Basket), `localhost:5246` (Payments).
RabbitMQ management at `localhost:15672`, Couchbase at `localhost:8091`.

## License

MIT
