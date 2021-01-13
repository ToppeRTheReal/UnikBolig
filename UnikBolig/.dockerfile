FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY UnikBolig.Api/*.csproj ./UnikBolig.Api/
COPY UnikBolig.Application/*.csproj ./UnikBolig.Application
COPY UnikBolig.Domain/*.csproj ./UnikBolig.Domain
COPY UnikBolig.Infrastructure/*.csproj ./UnikBolig
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/UnikBolig.Api/out .

# Insert name of project to be ran <aspnetapp.dll>
ENTRYPOINT ["dotnet", "UnikBolig.Api.dll"]