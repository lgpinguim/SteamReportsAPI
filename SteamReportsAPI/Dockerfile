#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SteamReportsAPI/SteamReportsAPI.csproj", "SteamReportsAPI/"]
RUN dotnet restore "SteamReportsAPI/SteamReportsAPI.csproj"
COPY . .
WORKDIR "/src/SteamReportsAPI"
RUN dotnet build "SteamReportsAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SteamReportsAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["SteamReportsAPI/steam.db", "/app/"]
ENV REDIS_HOST "redis"
ENV REDIS_PORT "6379"
ENTRYPOINT ["dotnet", "SteamReportsAPI.dll"]



