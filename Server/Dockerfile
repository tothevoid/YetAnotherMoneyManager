FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY . ./
RUN dotnet restore

COPY MoneyManager/. ./MoneyManager/
WORKDIR /app/MoneyManager
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as runtime
WORKDIR /app
COPY --from=build /app/MoneyManager/out ./
ENTRYPOINT ["dotnet", "MoneyManager.dll"]