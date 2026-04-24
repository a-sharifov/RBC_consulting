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
WORKDIR /src
COPY ["RBC_consulting.Api/RBC_consulting.csproj", "RBC_consulting.Api/"]
COPY ["RBC_consulting.Contracts/RBC_consulting.Contracts.csproj", "RBC_consulting.Contracts/"]
COPY ["RBC_consulting.Infrastructure/RBC_consulting.Infrastructure.csproj", "RBC_consulting.Infrastructure/"]
COPY ["RBC_consulting.Application/RBC_consulting.Application.csproj", "RBC_consulting.Application/"]
COPY ["RBC_consulting.Domain/RBC_consulting.Domain.csproj", "RBC_consulting.Domain/"]
RUN dotnet restore "./RBC_consulting.Api/RBC_consulting.csproj"
COPY . .
WORKDIR "/src/RBC_consulting.Api"
RUN dotnet build "./RBC_consulting.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RBC_consulting.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RBC_consulting.dll"]