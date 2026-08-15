namespace IdentityService.Application.DTOs;

public record CreateUserDto(string FullName, string Mobile);

public record UserResponseDto(Guid Id, string FullName, string Mobile, bool IsActive);

public record PagedResult<T>(IReadOnlyList<T> Items,
                             int PageNumber,
                             int PageSize,
                             int TotalCount)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}

public record GetUsersQuery(int PageNumber = 1, int PageSize = 10, bool? IsActive = null);
