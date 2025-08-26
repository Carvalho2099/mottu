# See https://docs.microsoft.com/dotnet/core/docker/build-container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["VehicleManagement.Api/VehicleManagement.Api.csproj", "VehicleManagement.Api/"]
RUN dotnet restore "VehicleManagement.Api/VehicleManagement.Api.csproj"
COPY . .
WORKDIR "/src/VehicleManagement.Api"
RUN dotnet build "VehicleManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VehicleManagement.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VehicleManagement.Api.dll"]
