# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de solución y proyectos
COPY *.sln ./
COPY SmartAccess.API/*.csproj ./SmartAccess.API/
COPY SmartAccess.Application/*.csproj ./SmartAccess.Application/
COPY SmartAccess.Domain/*.csproj ./SmartAccess.Domain/
COPY SmartAccess.Infrastructure/*.csproj ./SmartAccess.Infrastructure/
COPY SmartAccess.Shared/*.csproj ./SmartAccess.Shared/

# Restaurar dependencias
RUN dotnet restore

# Copiar el resto del código
COPY . .

# Publicar la app
WORKDIR /src/SmartAccess.API
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Exponer el puerto de la API
EXPOSE 80

# Comando de inicio
ENTRYPOINT ["dotnet", "SmartAccess.API.dll"]
