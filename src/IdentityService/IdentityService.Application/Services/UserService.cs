using IdentityService.Application.DTOs;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.Services;

public class UserService(IUserRepository _repository) : IUserService
{
    public async Task<UserResponseDto> CreateUserAsync(CreateUserDto dto, CancellationToken ct = default)
    {
        var user = new User(dto.FullName, dto.Mobile);
        await _repository.AddAsync(user, ct);
        await _repository.SaveChangesAsync(ct);
        return Map(user);
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(Guid id, CancellationToken ct = default)
    {
        var user = await _repository.GetByIdAsync(id, ct);
        return user is null ? null : Map(user);
    }

    public async Task<PagedResult<UserResponseDto>> GetUsersAsync(GetUsersQuery query, CancellationToken ct = default)
    {
        var (items, total) = await _repository.GetPagedAsync(query.PageNumber, query.PageSize, query.IsActive, ct);
        return new PagedResult<UserResponseDto>(items.Select(Map).ToList(), query.PageNumber, query.PageSize, total);
    }

    public Task<bool> UserExistsAsync(Guid id, CancellationToken ct = default) => _repository.ExistsAsync(id, ct);

    private static UserResponseDto Map(User u) => new(u.Id, u.FullName, u.Mobile, u.IsActive);
}
