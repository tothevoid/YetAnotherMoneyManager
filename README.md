# Personal money management system

Stack:

* React 19 + Vite + ChakraUI
* .NET 9
* Postgres 17.4
* minio (images)
* Docker (optional)

Development:

* install at least .NET 9
* install [postgres](https://www.postgresql.org/download/)
* install [Node.js](https://nodejs.org/en/)
* navigate to `./server/MoneyManager`
* execute `dotnet run`
* navigate to the directory `./client`
* execute `npm run dev`

Migrations:

* navigate to `./server`
* execute `dotnet ef database update --project Infrastructure --startup-project WebApi`

Start with Docker:

* Create `.env` by the `.env.example` reference
* Execute `docker-compose up --build -d`
