using Common.Middleware;
using Contracts.Grpc.Identity;
using Contracts.Grpc.Notification;
using EmployeeService.Application.DTOs;
using EmployeeService.Application.Interfaces;
using EmployeeService.Application.Services;
using EmployeeService.Grpc.Clients;
using EmployeeService.Infrastructure.Persistence;
using EmployeeService.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080, o => o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1);
});

builder.Services.AddDbContext<EmployeeDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// gRPC clients to Identity Service and Notification Service
builder.Services.AddGrpcClient<IdentityGrpc.IdentityGrpcClient>(o =>
{
    o.Address = new Uri(builder.Configuration["Grpc:IdentityServiceAddress"]!);
});

builder.Services.AddGrpcClient<NotificationGrpc.NotificationGrpcClient>(o =>
{
    o.Address = new Uri(builder.Configuration["Grpc:NotificationServiceAddress"]!);
});

builder.Services.AddScoped<IIdentityServiceClient, IdentityServiceClient>();
builder.Services.AddScoped<INotificationServiceClient, NotificationServiceClient>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IEmployeeService, ReservationService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Employee Service API (Reservation Management)",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Service API v1"));

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();
    db.Database.Migrate();
}

app.Run();
