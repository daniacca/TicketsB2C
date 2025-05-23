# 🎟️ TicketsB2C

**TicketsB2C** è un'applicazione backend modulare per la gestione di biglietteria B2C, sviluppata in .NET. Il progetto è strutturato per supportare componenti riutilizzabili, test automatizzati e ambienti containerizzati tramite Docker.

## 📂 Struttura del progetto

- **TicketsB2C**: Applicazione principale.
- **TicketsB2C.Services**: Servizi core dell'applicazione.
- **TicketsB2C.Services.Tests**: Test unitari per i servizi core.
- **Discount**: Modulo per la gestione degli sconti.
- **Discount.Services**: Logica di business relativa agli sconti.
- **Discount.Services.Tests**: Test unitari per i servizi di sconto.
- **DataAccess**: Gestione dell'accesso ai dati e migrazioni.
- **Migrations**: Script per la gestione delle migrazioni del database.

## 🚀 Avvio rapido

### Prerequisiti

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)

### Clonazione del repository

```bash
git clone https://github.com/daniacca/TicketsB2C.git
cd TicketsB2C
````

### Avvio con Docker Compose

```bash
docker-compose up --build
```

### Esecuzione dei test

```bash
dotnet test
```

## ⚙️ Comandi utili

* **Build dell'applicazione**:

  ```bash
  dotnet build TicketsB2C.sln
  ```

* **Esecuzione dell'applicazione**:

  ```bash
  dotnet run --project TicketsB2C/TicketsB2C.csproj
  ```

* **Avvio dei container Docker**:

  ```bash
  docker-compose up
  ```

* **Pulizia dei container Docker**:

  ```bash
  docker-compose down
  ```

## 🧪 Test

I test automatizzati sono presenti nei seguenti progetti:

* `TicketsB2C.Services.Tests`
* `Discount.Services.Tests`

Per eseguire tutti i test:

```bash
dotnet test
```

## 📄 Licenza

Questo progetto è distribuito sotto licenza MIT. Per maggiori dettagli, consultare il file [LICENSE.txt](LICENSE.txt).
