using Contracts.Grpc.Identity;
using EmployeeService.Application.Interfaces;

namespace EmployeeService.Grpc.Clients;

public class IdentityServiceClient(IdentityGrpc.IdentityGrpcClient _client) : IIdentityServiceClient
{
    public async Task<bool> UserExistsAsync(Guid userId, CancellationToken ct = default)
    {
        var response = await _client.UserExistsAsync(new UserExistsRequest { UserId = userId.ToString() },
                                                     cancellationToken: ct);

        return response.Exists;
    }
}
