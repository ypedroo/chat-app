FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Jobsity.Chat/Jobsity.Chat.csproj", "Jobsity.Chat/"]
COPY ["Jobsity.Chat.Repositories/Jobsity.Chat.Repositories.csproj", "Jobsity.Chat.Repositories/"]
COPY ["Jobsity.Chat.Domain/Jobsity.Chat.Domain.csproj", "Jobsity.Chat.Domain/"]
COPY ["Jobsity.Chat.Services/Jobsity.Chat.Services.csproj", "Jobsity.Chat.Services/"]
COPY ["Jobsity.Chat.Bot/Jobsity.Chat.Bot.csproj", "Jobsity.Chat.Bot/"]
RUN dotnet restore "Jobsity.Chat/Jobsity.Chat.csproj"
COPY . .
WORKDIR "/src/Jobsity.Chat"
RUN dotnet build "Jobsity.Chat.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Jobsity.Chat.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jobsity.Chat.dll"]
