## Multi-stage build for CoreHost ASP.NET Core 8 app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj first for better layer caching
COPY CoreHost/CoreHostApp/CoreHostApp.csproj CoreHost/CoreHostApp/
RUN dotnet restore CoreHost/CoreHostApp/CoreHostApp.csproj

# copy the rest of the source
COPY . .
WORKDIR /src/CoreHost/CoreHostApp
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CoreHostApp.dll"]
