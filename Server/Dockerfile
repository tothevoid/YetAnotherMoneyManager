FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet restore

COPY MoneyManager/. ./MoneyManager/
WORKDIR /app/MoneyManager
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/MoneyManager/out ./
ENTRYPOINT ["dotnet", "MoneyManager.dll"]