# Re Az VPN Bot

A Persian-language Telegram bot built with .NET 10, MediatR, and EF Core with SQLite.

Send `/start` in a private chat to open the account dashboard. It shows your name, username, and Telegram ID. The **Buy VPN** and **Purchase History** buttons open views within the same message. Both views currently show placeholders.

## Configure the bot

On a fresh checkout, copy the [environment template](.env.example) from the project root.

```sh
cp .env.example .env
```

Set `BOT_TOKEN` in `.env` to the token you received from Telegram's `@BotFather`.

## Run locally

Install the .NET 10 SDK and run the bot from the project root.

```sh
dotnet run --project Bot.csproj
```

Press Enter to stop the bot.

## Run with Docker Compose

With Docker running, build and start the bot.

```sh
docker compose up --build -d
docker compose logs -f bot
```

To stop the bot, run `docker compose down`.

## SQLite storage

| Environment | Database path |
| --- | --- |
| Local | `bot.db` in the working directory |
| Docker Compose | `/data/bot.db` in the `bot-data` volume |

For local runs, set `BOT_DB_CONNECTION_STRING` to use a different path. The default connection string is `Data Source=bot.db`. Docker Compose sets its own connection string and keeps the database in the volume between container recreations. See [compose.yaml](compose.yaml).

Startup opens the database and creates the file when needed. The current EF Core model has an empty schema. The first migration can be generated when the first entity is added.

## Project layout

| Path | Purpose |
| --- | --- |
| [`Program.cs`](Program.cs) | Configuration, dependency injection, and Telegram event subscriptions |
| [`Core/`](Core/) | Message and callback routing |
| [`UseCases/`](UseCases/) | MediatR requests and handlers |
| [`Pages/`](Pages/) | Dashboard captions and keyboards |
| [`Data/`](Data/) | EF Core database context |
