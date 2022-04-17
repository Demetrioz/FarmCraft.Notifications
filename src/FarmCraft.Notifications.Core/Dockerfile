#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FarmCraft.Notifications.Core/FarmCraft.Notifications.Core.csproj", "FarmCraft.Notifications.Core/"]
RUN dotnet restore "FarmCraft.Notifications.Core/FarmCraft.Notifications.Core.csproj"
COPY . .
WORKDIR "/src/FarmCraft.Notifications.Core"
RUN dotnet build "FarmCraft.Notifications.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FarmCraft.Notifications.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FarmCraft.Notifications.Core.dll"]