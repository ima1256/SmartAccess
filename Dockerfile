# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivos de proyecto
COPY *.sln ./
COPY SmartAccess.API/*.csproj SmartAccess.API/
COPY SmartAccess.Application/*.csproj SmartAccess.Application/
COPY SmartAccess.Domain/*.csproj SmartAccess.Domain/
COPY SmartAccess.Infrastructure/*.csproj SmartAccess.Infrastructure/
COPY SmartAccess.Shared/*.csproj SmartAccess.Shared/
COPY SmartAccess.Tests/*.csproj SmartAccess.Tests/

# Restaurar dependencias
RUN dotnet restore

# Copiar todo y compilar
COPY . .
WORKDIR /app/SmartAccess.API
RUN dotnet publish -c Release -o /out

# Etapa 2: Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "SmartAccess.API.dll"]
