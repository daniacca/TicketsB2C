# Vedere https://aka.ms/customizecontainer per informazioni su come personalizzare il contenitore di debug e su come Visual Studio usa questo Dockerfile per compilare le immagini per un debug più rapido.

# Questa fase viene usata durante l'esecuzione da Visual Studio in modalità rapida (impostazione predefinita per la configurazione di debug)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Questa fase viene usata per compilare il progetto di servizio
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TicketsB2C/TicketsB2C.csproj", "TicketsB2C/"]
COPY ["TicketsModel/TicketsB2C.Model.csproj", "TicketsModel/"]
COPY ["TicketsB2C.Services/TicketsB2C.Services.csproj", "TicketsB2C.Services/"]
RUN dotnet restore "./TicketsB2C/TicketsB2C.csproj"
COPY . .
WORKDIR "/src/TicketsB2C"
RUN dotnet build "./TicketsB2C.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Questa fase viene usata per pubblicare il progetto di servizio da copiare nella fase finale
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TicketsB2C.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Questa fase viene usata nell'ambiente di produzione o durante l'esecuzione da Visual Studio in modalità normale (impostazione predefinita quando non si usa la configurazione di debug)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicketsB2C.dll"]