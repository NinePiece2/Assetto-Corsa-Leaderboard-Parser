# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Assetto-Corsa-Leaderboard-Parser/Assetto-Corsa-Leaderboard-Parser.csproj", "Assetto-Corsa-Leaderboard-Parser/"]
RUN dotnet restore "./Assetto-Corsa-Leaderboard-Parser/Assetto-Corsa-Leaderboard-Parser.csproj"
COPY . .
WORKDIR "/src/Assetto-Corsa-Leaderboard-Parser"
RUN dotnet build "./Assetto-Corsa-Leaderboard-Parser.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Assetto-Corsa-Leaderboard-Parser.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Assetto-Corsa-Leaderboard-Parser.dll"]