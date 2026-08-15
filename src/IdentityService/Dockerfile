# ---------- Build Stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/IdentityService/IdentityService.Api/IdentityService.Api.csproj src/IdentityService/IdentityService.Api/
COPY src/IdentityService/IdentityService.Application/IdentityService.Application.csproj src/IdentityService/IdentityService.Application/
COPY src/IdentityService/IdentityService.Domain/IdentityService.Domain.csproj src/IdentityService/IdentityService.Domain/
COPY src/IdentityService/IdentityService.Infrastructure/IdentityService.Infrastructure.csproj src/IdentityService/IdentityService.Infrastructure/
COPY shared/Contracts.Grpc/Contracts.Grpc.csproj shared/Contracts.Grpc/
COPY shared/Common/Common.csproj shared/Common/

RUN dotnet restore src/IdentityService/IdentityService.Api/IdentityService.Api.csproj

COPY src/IdentityService/ src/IdentityService/
COPY shared/ shared/

WORKDIR /src/src/IdentityService/IdentityService.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

# ---------- Runtime Stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
EXPOSE 6001

ENTRYPOINT ["dotnet", "IdentityService.Api.dll"]
