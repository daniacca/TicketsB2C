# 🎟️ TicketsB2C

**TicketsB2C** is a modular .NET backend application for B2C ticket management. The system is composed of multiple services communicating over HTTP, with SQL Server as the database engine. The project is structured to support reusable components, automated testing, and containerized environments via Docker.

---

## 📦 Architecture Overview

- **TicketsB2C**: Main application, exposes HTTP endpoints and connects to SQL Server.
- **Discount API**: A secondary service used internally via HTTP by TicketsB2C.
- **SQL Server**: Relational database used by TicketsB2C for persistence.

All services are orchestrated via `docker-compose` for local development and testing.

---

## 📂 Project Structure

```

TicketsB2C/
├── TicketsB2C/                  # Main application
├── TicketsB2C.Services/         # Core services
├── TicketsB2C.Services.Tests/   # Unit tests for core services
├── Discount/                    # Discount service (Web API)
├── Discount.Services/           # Business logic for discounts
├── Discount.Services.Tests/     # Unit tests for discounts
├── Discount.Tests/              # E2E tests for Discount service
├── DataAccess/                  # EF Core DB context and models
├── Migrations/                  # EF Core migrations runner
└── docker-compose.yml

````

---

## 🚀 Quick Start

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Docker + Docker Compose](https://docs.docker.com/compose/)

### Run all services

```bash
make docker-build
make up
````

This will:

* Build and start:

  * SQL Server
  * Discount API
  * (Optionally) TicketsB2C, if Dockerized
* TicketsB2C will expect the Discount service to be reachable at `http://discount-web-api.io:8082`

> 🔁 Connection strings and API base URLs are configured via environment variables or `appsettings`.

---

## 🧪 Testing

The repository includes both **unit** and **end-to-end** tests:

* `TicketsB2C.Services.Tests`: Unit tests for business logic
* `Discount.Services.Tests`: Unit tests for discounts
* `Discount.Tests`: End-to-end tests for Discount API

Run all tests:

```bash
make test
```

---

## 🛠️ Makefile Commands

The included `Makefile` simplifies common tasks:

| Command                                 | Description                          |
| --------------------------------------- | ------------------------------------ |
| `make clean`                            | Clean build artifacts                |
| `make restore`                          | Restore NuGet packages               |
| `make build`                            | Build main project                   |
| `make build-all`                        | Build the full solution              |
| `make publish`                          | Publish for deployment               |
| `make test`                             | Run all tests                        |
| `make docker-build`                     | Build Docker images for all services |
| `make up`                               | Start services via Docker Compose    |
| `make down`                             | Stop all Docker services             |
| `make add-migration MIGRATION_NAME=...` | Add EF Core DB migration             |

---

## 🧱 Database Migrations

To add a new database migration using EF Core:

```bash
make add-migration MIGRATION_NAME=AddNewTable
```

---

## 📄 License

This project is licensed under the MIT License. See [LICENSE.txt](LICENSE.txt) for details.
