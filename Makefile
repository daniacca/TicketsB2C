.PHONY: add-migration

MIGRATION_NAME=

DATA_ACCESS_PROJECT=./DataAccess/TicketsModel.csproj
STARTUP_PROJECT=./Migrations/Migrations.csproj

add-migration:
	@if [ -z "$(MIGRATION_NAME)" ]; then \
		echo "Errore: Specificare il nome della migrazione usando MIGRATION_NAME=<nome>"; \
		exit 1; \
	fi
	dotnet ef migrations add $(MIGRATION_NAME) --project $(DATA_ACCESS_PROJECT) --startup-project $(STARTUP_PROJECT)
