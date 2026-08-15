namespace EmployeeService.Application.Interfaces;

public interface IIdentityServiceClient
{
    Task<bool> UserExistsAsync(Guid userId, CancellationToken ct = default);
}
