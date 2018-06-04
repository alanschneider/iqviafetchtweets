FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY IqviaFetchTweets/IqviaFetchTweets.csproj IqviaFetchTweets/
RUN dotnet restore IqviaFetchTweets/IqviaFetchTweets.csproj
COPY . .
WORKDIR /src/IqviaFetchTweets
RUN dotnet build IqviaFetchTweets.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish IqviaFetchTweets.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IqviaFetchTweets.dll"]
