using System.Text.Json;
using EmployeeService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Infrastructure.Persistence;

public class EmployeeDbContext : DbContext
{
    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

    public DbSet<Employee> Reservations => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(builder =>
        {
            builder.ToTable("Reservations");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.UserId).IsRequired();
            builder.Property(r => r.Department).IsRequired();
            builder.Property(r => r.Position).IsRequired();
            builder.Property(r => r.EmploymentDate).IsRequired();

            // Preferences stored as jsonb
            builder.Property(r => r.Preferences)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<EmployeePreferences>(v, (JsonSerializerOptions?)null) ?? new EmployeePreferences());

            builder.HasIndex(r => r.Position);
        });
    }
}
