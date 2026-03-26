# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and projects
COPY ../*.sln ./
COPY ../MiniPricingPlatform.API/*.csproj ./MiniPricingPlatform.API/
COPY ../MiniPricingPlatform.Application/*.csproj ./MiniPricingPlatform.Application/
COPY ../MiniPricingPlatform.Domain/*.csproj ./MiniPricingPlatform.Domain/
COPY ../MiniPricingPlatform.Infrastructure/*.csproj ./MiniPricingPlatform.Infrastructure/
COPY ../MiniPricingPlatform.Tests/*.csproj ./MiniPricingPlatform.Tests/

# Restore dependencies
RUN dotnet restore

# Copy all source code
COPY ../MiniPricingPlatform.API ./MiniPricingPlatform.API
COPY ../MiniPricingPlatform.Application ./MiniPricingPlatform.Application
COPY ../MiniPricingPlatform.Domain ./MiniPricingPlatform.Domain
COPY ../MiniPricingPlatform.Infrastructure ./MiniPricingPlatform.Infrastructure
COPY ../MiniPricingPlatform.Tests ./MiniPricingPlatform.Tests

WORKDIR /app/MiniPricingPlatform.API
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5550
ENTRYPOINT ["dotnet", "MiniPricingPlatform.API.dll"]