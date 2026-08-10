# Personal money management system

Stack:

* React 19 + Vite + ChakraUI
* .NET 10
* Postgres 17.4
* minio
* Docker

Development:

* install docker
* execute `docker compose -p moneymanager_dev --env-file .env.dev up -d --build`
* remove client and server services
* install at least .NET 10
* install [Node.js](https://nodejs.org/en/)
* navigate to `./server/WebApi`
* execute `dotnet run`
* navigate to the directory `./client`
* execute `npm run dev`

Deploy:

* install docker
* create `.env` by the `.env.example` reference
* execute `docker compose -p moneymanager up --build -d`

Migrations:

* dotnet tool install --global dotnet-ef --version 10.*
* navigate to `./server`
* execute `dotnet ef database update --project Infrastructure --startup-project WebApi`