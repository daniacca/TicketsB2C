.PHONY: clean restore build build-all publish test docker-build up down add-migration

CONFIG ?= Debug
COVERAGE ?= false
PROJECT ?= TicketsB2C/TicketsB2C.csproj
SOLUTION ?= TicketsB2C.sln
DATA_ACCESS_PROJECT = ./DataAccess/TicketsModel.csproj
STARTUP_PROJECT = ./Migrations/Migrations.csproj
MIGRATION_NAME ?=

clean:
	dotnet clean

restore:
	dotnet restore

build:
	dotnet build $(PROJECT) --configuration $(CONFIG)

build-all:
	dotnet build $(SOLUTION) --configuration $(CONFIG)

publish:
	dotnet publish $(PROJECT) --configuration Release --output ./publish

test:
ifeq ($(COVERAGE), true)
	dotnet test --collect:"XPlat Code Coverage"
else
	dotnet test
endif

docker-build:
	docker-compose build

up:
	docker-compose up

down:
	docker-compose down

add-migration:
	@if [ -z "$(MIGRATION_NAME)" ]; then \
		echo "Error: Please specify a migration name using MIGRATION_NAME=<name>"; \
		exit 1; \
	fi
	dotnet ef migrations add $(MIGRATION_NAME) --project $(DATA_ACCESS_PROJECT) --startup-project $(STARTUP_PROJECT)
