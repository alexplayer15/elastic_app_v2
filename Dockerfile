FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
ARG PROJECT_NAME="elastic_app.api.csproj"

# copy csproj and restore as distinct layers
COPY backend/src/elastic_app.api/elastic_app.api.csproj backend/src/elastic_app.api/
COPY backend/src/elastic_app.application/elastic_app.application.csproj backend/src/elastic_app.application/
COPY backend/src/elastic_app.domain/elastic_app.domain.csproj backend/src/elastic_app.domain/
COPY backend/src/elastic_app.infrastructure/elastic_app.infrastructure.csproj backend/src/elastic_app.infrastructure/

RUN dotnet restore backend/src/elastic_app.api/elastic_app.api.csproj

# copy and build app and libraries
COPY backend/src/elastic_app.api/ backend/src/elastic_app.api/
COPY backend/src/elastic_app.application/ backend/src/elastic_app.application/
COPY backend/src/elastic_app.domain/ backend/src/elastic_app.domain/
COPY backend/src/elastic_app.infrastructure/ backend/src/elastic_app.infrastructure/

WORKDIR /source/backend/src/elastic_app.api
RUN dotnet build $PROJECT_NAME -c Release -o /app/build

FROM build AS unit-tests
WORKDIR /source
COPY backend/test/elastic_app.api.tests/ backend/test/elastic_app.api.tests/
WORKDIR source/backend/test/elastic_app.api.tests/
RUN dotnet test -c Release --logger trx --results-directory /app/TestResults/ .

FROM scratch AS unit-test-results
COPY --from=unit-tests /app/TestResults/*.trx .

FROM build AS publish
RUN dotnet publish $PROJECT_NAME -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "elastic_app.api.dll"]