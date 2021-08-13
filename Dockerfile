FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY Lapka.Identity.Api/Lapka.Identity.Api.csproj Lapka.Identity.Api/Lapka.Identity.Api.csproj
COPY Lapka.Identity.Application/Lapka.Identity.Application.csproj Lapka.Identity.Application/Lapka.Identity.Application.csproj
COPY Lapka.Identity.Core/Lapka.Identity.Core.csproj Lapka.Identity.Core/Lapka.Identity.Core.csproj
COPY Lapka.Identity.Infrastructure/Lapka.Identity.Infrastructure.csproj Lapka.Identity.Infrastructure/Lapka.Identity.Infrastructure.csproj
RUN dotnet restore Lapka.Identity.Api

COPY . .
RUN dotnet publish Lapka.Identity.Api -c release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS http://*:5001
ENV ASPNETCORE_ENVIRONMENT Docker

EXPOSE 5001

ENTRYPOINT dotnet Lapka.Identity.Api.dll