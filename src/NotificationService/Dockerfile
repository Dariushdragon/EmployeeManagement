# ---------- Build Stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/NotificationService/NotificationService.Api/NotificationService.Api.csproj src/NotificationService/NotificationService.Api/
COPY src/NotificationService/NotificationService.Application/NotificationService.Application.csproj src/NotificationService/NotificationService.Application/
COPY src/NotificationService/NotificationService.Domain/NotificationService.Domain.csproj src/NotificationService/NotificationService.Domain/
COPY src/NotificationService/NotificationService.Infrastructure/NotificationService.Infrastructure.csproj src/NotificationService/NotificationService.Infrastructure/
COPY shared/Contracts.Grpc/Contracts.Grpc.csproj shared/Contracts.Grpc/
COPY shared/Common/Common.csproj shared/Common/

RUN dotnet restore src/NotificationService/NotificationService.Api/NotificationService.Api.csproj

COPY src/NotificationService/ src/NotificationService/
COPY shared/ shared/

WORKDIR /src/src/NotificationService/NotificationService.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

# ---------- Runtime Stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
EXPOSE 6002

ENTRYPOINT ["dotnet", "NotificationService.Api.dll"]
