# 🎟️ TicketsB2C

**TicketsB2C** is a modular .NET backend application for B2C ticket management. The project is structured to support reusable components, automated testing, and containerized environments via Docker.

## 📂 Project Structure

- **TicketsB2C**: Main application.
- **TicketsB2C.Services**: Core business services.
- **TicketsB2C.Services.Tests**: Unit tests for core services.
- **Discount**: Discount management module.
- **Discount.Services**: Business logic related to discounts.
- **Discount.Services.Tests**: Unit tests for the discount services.
- **DataAccess**: Data access layer and database migrations.
- **Migrations**: Scripts for managing database schema changes.

## 🚀 Quick Start

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)

### Clone the Repository

```bash
git clone https://github.com/daniacca/TicketsB2C.git
cd TicketsB2C
````

### Run with Docker Compose

```bash
docker-compose up --build
```

### Run Tests

```bash
dotnet test
```

## ⚙️ Useful Commands

* **Build the solution**:

  ```bash
  dotnet build TicketsB2C.sln
  ```

* **Run the application**:

  ```bash
  dotnet run --project TicketsB2C/TicketsB2C.csproj
  ```

* **Start Docker containers**:

  ```bash
  docker-compose up
  ```

* **Stop and remove Docker containers**:

  ```bash
  docker-compose down
  ```

## 🧪 Testing

Automated unit tests are located in:

* `TicketsB2C.Services.Tests`
* `Discount.Services.Tests`

To run all tests:

```bash
dotnet test
```

## 📄 License

This project is licensed under the MIT License. For more information, see the [LICENSE.txt](LICENSE.txt) file.
