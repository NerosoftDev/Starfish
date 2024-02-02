#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["NuGet.config", "."]
COPY ["Source/Starfish.Webapi/Starfish.Webapi.csproj", "Source/Starfish.Webapi/"]
COPY ["Source/Starfish.Service/Starfish.Service.csproj", "Source/Starfish.Service/"]
COPY ["Source/Starfish.Common/Starfish.Common.csproj", "Source/Starfish.Common/"]
COPY ["Source/Starfish.Transit/Starfish.Transit.csproj", "Source/Starfish.Transit/"]
RUN dotnet restore "./Source/Starfish.Webapi/./Starfish.Webapi.csproj"
COPY . .
WORKDIR "/src/Source/Starfish.Webapi"
RUN dotnet build "./Starfish.Webapi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Starfish.Webapi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Starfish.Webapi.dll"]