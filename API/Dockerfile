#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["API/EAuction.API.csproj", "API/"]
COPY ["Model/EAuction.Domain.csproj", "Model/"]
COPY ["EAuction.DataAccessProvider/EAuction.DataAccessProvider.csproj", "EAuction.DataAccessProvider/"]
COPY ["Services/EAuction.Service.csproj", "Services/"]
RUN dotnet restore "API/EAuction.API.csproj"
COPY . .
WORKDIR "/src/API"
RUN dotnet build "EAuction.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EAuction.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EAuction.API.dll"]