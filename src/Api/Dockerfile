# docker build -f src/Api/Dockerfile . -t the-app-api
# docker run --name the-app -di -p 8080:80 the-app-api
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Api/Api.csproj", "Api/"]
COPY ["src/App.Data.Sqlite/App.Data.Sqlite.csproj", "App.Data.Sqlite/"]
COPY ["src/App.Data.Postgres/App.Data.Postgres.csproj", "App.Data.Postgres/"]
COPY ["src/App.Data/App.Data.csproj", "App.Data/"]
COPY ["src/App.Data.SqlServer/App.Data.SqlServer.csproj", "App.Data.SqlServer/"]
COPY ["src/App.Services/App.Services.csproj", "App.Services/"]
COPY ["src/FastApi.Endpoints/FastApi.Endpoints.csproj", "FastApi.Endpoints/"]
COPY ["src/FastApi.EF/FastApi.EF.csproj", "FastApi.EF/"]
RUN dotnet restore "Api/Api.csproj"
COPY ./src .
RUN dotnet build "Api/Api.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "Api/Api.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]