using IdentityService.Application.DTOs;

namespace IdentityService.Application.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> CreateUserAsync(CreateUserDto dto, CancellationToken ct = default);
    Task<UserResponseDto?> GetUserByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<UserResponseDto>> GetUsersAsync(GetUsersQuery query, CancellationToken ct = default);
    Task<bool> UserExistsAsync(Guid id, CancellationToken ct = default);
}
