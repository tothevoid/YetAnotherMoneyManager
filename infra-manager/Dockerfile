FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["InfrastructureManager.Application/InfrastructureManager.Application.csproj", "InfrastructureManager.Application/"]
COPY ["InfrastructureManager.WebApi/InfrastructureManager.WebApi.csproj", "InfrastructureManager.WebApi/"]
RUN dotnet restore "InfrastructureManager.WebApi/InfrastructureManager.WebApi.csproj"

COPY . .
WORKDIR "/src/InfrastructureManager.WebApi"
RUN dotnet publish "InfrastructureManager.WebApi.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
RUN apt-get update && apt-get install -y --no-install-recommends curl ca-certificates gnupg lsb-release \
    && install -d /etc/apt/keyrings \
    && curl -fsSL https://www.postgresql.org/media/keys/ACCC4CF8.asc | gpg --dearmor -o /etc/apt/keyrings/postgresql.gpg \
    && echo "deb [signed-by=/etc/apt/keyrings/postgresql.gpg] http://apt.postgresql.org/pub/repos/apt $(lsb_release -cs)-pgdg main" > /etc/apt/sources.list.d/pgdg.list \
    && apt-get update \
    && apt-get install -y --no-install-recommends postgresql-client-17 \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app/build ./

EXPOSE 5050
ENTRYPOINT ["dotnet", "InfrastructureManager.WebApi.dll"]
