FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app
COPY . ./
RUN dotnet restore "./MoneyManager/MoneyManager.csproj"
RUN dotnet publish "./MoneyManager/MoneyManager.csproj" -c Production -o out
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/MoneyManager/out .
ENTRYPOINT ["dotnet", "MoneyManager.dll"]