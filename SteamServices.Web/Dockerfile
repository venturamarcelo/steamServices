#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SteamServices.Web/SteamServices.Web.csproj", "SteamServices.Web/"]
RUN dotnet restore "SteamServices.Web/SteamServices.Web.csproj"
COPY . .
WORKDIR "/src/SteamServices.Web"
RUN dotnet build "SteamServices.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SteamServices.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SteamServices.Web.dll"]