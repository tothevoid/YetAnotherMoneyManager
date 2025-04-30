# Personal money management system
Stack:

* React 19 + Vite + ChakraUI
* .NET 9
* Postgres 17.4
* Docker (optional)

Development:

* install at least .NET 9
* install [postgres](https://www.postgresql.org/download/)
* install [Node.js](https://nodejs.org/en/)
* Navigate to the directory `./Server/MoneyManager`
* Execute `dotnet run`
* Navigate to the directory `./Client`
* Execute `npm run dev`

Migrations:

`dotnet ef database update --project Infrastructure --startup-project WebApi`

Start with Docker:

* Create `.env` by the `.env.example` reference
* Execute `docker-compose up --build -d`
