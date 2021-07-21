#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 5007
EXPOSE 5057
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Presentation/Utilitary.API/Utilitary.API.csproj", "src/Presentation/Utilitary.API/"]
COPY ["src/Infrastructure/Utilitary.Persistence/Utilitary.Persistence.csproj", "src/Infrastructure/Utilitary.Persistence/"]
COPY ["src/Core/Utilitary.Core/Utilitary.Core.csproj", "src/Core/Utilitary.Core/"]
COPY ["src/Domain/Utilitary.Domain/Utilitary.Domain.csproj", "src/Domain/Utilitary.Domain/"]
RUN dotnet restore "src/Presentation/Utilitary.API/Utilitary.API.csproj"
COPY . .
WORKDIR "/src/src/Presentation/Utilitary.API"
RUN dotnet build "Utilitary.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Utilitary.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ibero.Services.Utilitary.API.dll"]
