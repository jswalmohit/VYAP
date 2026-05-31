# Use the official .NET SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore as a separate step to leverage Docker cache
COPY VyapSetuAPI.sln ./
COPY ShopManagementSystem.API/ShopManagementSystem.API.csproj ./ShopManagementSystem.API/
COPY ShopManagementSystem.Application/ShopManagementSystem.Application.csproj ./ShopManagementSystem.Application/
COPY ShopManagementSystem.Infrastructure/ShopManagementSystem.Infrastructure.csproj ./ShopManagementSystem.Infrastructure/
COPY ShopManagementSystem.Domain/ShopManagementSystem.Domain.csproj ./ShopManagementSystem.Domain/

RUN dotnet restore VyapSetuAPI.sln

# Copy everything else and build/publish the project
COPY . .
WORKDIR /src/ShopManagementSystem.API
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENV PORT=80
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "ShopManagementSystem.API.dll"]
