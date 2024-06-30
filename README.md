# .NET Challenge


## Description

This project is designed to test your knowledge of back-end web technologies, specifically in
.NET and assess your ability to create back-end products with attention to details, standards,
and reusability.

## Assignment

The goal of this exercise is to create a simple browser-based chat application using .NET.

This application should allow several users to talk in a chatroom and also to get stock quotes
from an API using a specific command.

## Mandatory Features

- Allow registered users to log in and talk with other users in a chatroom.
- Allow users to post messages as commands into the chatroom with the following format
/stock=stock_code
- Create a decoupled bot that will call an API using the stock_code as a parameter
(https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv, here aapl.us is the
stock_code)
- The bot should parse the received CSV file and then it should send a message back into
the chatroom using a message broker like RabbitMQ. The message will be a stock quote
using the following format: APPL.US quote is $93.42 per share. The post owner will be
the bot.
- Have the chat messages ordered by their timestamps and show only the last 50
messages.
- Unit test the functionality you prefer.

## Bonus (Optional)

- [x] Have more than one chatroom.
- [x] Use .NET identity for users authentication
- [x] Handle messages that are not understood or any exceptions raised within the bot.
- [ ] Build an installer, this is missing never built wix using MacOS 
- [ ] Docker Compose service: Incomplete, due to time i was not able to finish the config and create the network, that is why we need the how to run session. 

## How to run
- Create RabbitMq container:
```bash
docker run -d --hostname rmq --name rabbit-server -p 8080:15672 -p 5672:5672 rabbitmq:3-management
```
- Create Sql Server container:
```bash
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Str0ngPa$$w0rd' -e 'MSSQL_PID=Express' -p 1433:1433 -d mcr.microsoft.com/mssql/server
```
- Run migrations:
```bash
dotnet ef database update
```
- Run the project:
```bash
dotnet run
```

## Considerations

- We will open 2 browser windows and log in with 2 different users to test the
functionalities.
- The stock command won't be saved on the database as a post.
- The project is totally focused on the backend; please have the frontend as simple as you
can.
- Keep confidential information secure.
- Pay attention if your chat is consuming too many resources.
- Keep your code versioned with Git locally.
- Feel free to use small helper libraries
