FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet publish src/WebApi/WebApi.csproj -c Release -o build

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/build ./
ENTRYPOINT ["dotnet", "Audex.WebApi.dll"]