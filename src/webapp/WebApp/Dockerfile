# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /repo
COPY ["Directory.Packages.props", "./"]
COPY ["src/webapp/WebApp/WebApp.Api.csproj", "src/webapp/WebApp/"]
COPY ["src/webapp/WebApp.Contracts/WebApp.Contracts.csproj", "src/webapp/WebApp.Contracts/"]
COPY ["src/webapp/WebApp.Infrastructure/WebApp.Infrastructure.csproj", "src/webapp/WebApp.Infrastructure/"]
COPY ["src/webapp/WebApp.Application/WebApp.Application.csproj", "src/webapp/WebApp.Application/"]
COPY ["src/webapp/WebApp.Domain/WebApp.Domain.csproj", "src/webapp/WebApp.Domain/"]
RUN dotnet restore "src/webapp/WebApp/WebApp.Api.csproj"
COPY . .
WORKDIR "/repo/src/webapp/WebApp"
RUN dotnet build "WebApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApp.Api.dll"]
