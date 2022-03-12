# Stage 1 - Build

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /app

RUN apt-get update \
    && apt-get install -y --no-install-recommends unzip \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg

# Copy .sln + .csproj and restore as a distinct layer
COPY *.sln .
COPY TaxCalculator/*.csproj ./TaxCalculator/
COPY TaxCalculator.Tests/*.csproj ./TaxCalculator.Tests/
COPY TaxCalculator.IntegrationTests/*.csproj ./TaxCalculator.IntegrationTests/
RUN dotnet restore

# Copy over everything else and build app
COPY TaxCalculator/. ./TaxCalculator/
COPY TaxCalculator.Tests/. ./TaxCalculator.Tests/

RUN dotnet build -c Debug -o out
WORKDIR /app/TaxCalculator.Tests
RUN dotnet test

# Stage 2 - Debug

FROM build as debug

WORKDIR /app/TaxCalculator/bin/Debug/TaxCalculator/net5.0

ENTRYPOINT ["dotnet", "exec", "TaxCalculator.dll", "--urls", "http://0.0.0.0:5100"]

# Stage 3 - Release

FROM debug as release

WORKDIR /app/TaxCalculator

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=release /app/TaxCalculator/out ./

ENTRYPOINT ["dotnet", "TaxCalculator.dll", "--urls", "http://0.0.0.0:5100"]
