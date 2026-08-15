# ---------- Build Stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/EmployeeService/EmployeeService.Api/EmployeeService.Api.csproj src/EmployeeService/EmployeeService.Api/
COPY src/EmployeeService/EmployeeService.Application/EmployeeService.Application.csproj src/EmployeeService/EmployeeService.Application/
COPY src/EmployeeService/EmployeeService.Domain/EmployeeService.Domain.csproj src/EmployeeService/EmployeeService.Domain/
COPY src/EmployeeService/EmployeeService.Infrastructure/EmployeeService.Infrastructure.csproj src/EmployeeService/EmployeeService.Infrastructure/
COPY src/EmployeeService/EmployeeService.Grpc.Clients/EmployeeService.Grpc.Clients.csproj src/EmployeeService/EmployeeService.Grpc.Clients/
COPY shared/Contracts.Grpc/Contracts.Grpc.csproj shared/Contracts.Grpc/
COPY shared/Common/Common.csproj shared/Common/

RUN dotnet restore src/EmployeeService/EmployeeService.Api/EmployeeService.Api.csproj

COPY src/EmployeeService/ src/EmployeeService/
COPY shared/ shared/

WORKDIR /src/src/EmployeeService/EmployeeService.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

# ---------- Runtime Stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "EmployeeService.Api.dll"]
