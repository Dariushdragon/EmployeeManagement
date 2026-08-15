using Contracts.Grpc.Identity;
using Grpc.Core;
using IdentityService.Application.Interfaces;

namespace IdentityService.Api.Grpc;

public class UserGrpcService : IdentityGrpc.IdentityGrpcBase
{
    private readonly IUserService _userService;

    public UserGrpcService(IUserService userService)
    {
        _userService = userService;
    }

    public override async Task<UserExistsResponse> UserExists(UserExistsRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.UserId, out var userId))
            return new UserExistsResponse { Exists = false };

        var exists = await _userService.UserExistsAsync(userId, context.CancellationToken);
        return new UserExistsResponse { Exists = exists };
    }

    public override async Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.UserId, out var userId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid userId format."));

        var user = await _userService.GetUserByIdAsync(userId, context.CancellationToken);
        if (user is null)
            throw new RpcException(new Status(StatusCode.NotFound, "User not found."));

        return new GetUserResponse
        {
            Id = user.Id.ToString(),
            FullName = user.FullName,
            Mobile = user.Mobile,
            IsActive = user.IsActive
        };
    }
}
