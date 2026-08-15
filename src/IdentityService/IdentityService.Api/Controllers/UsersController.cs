using Common;
using IdentityService.Application.DTOs;
using IdentityService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto, CancellationToken ct)
    {
        var result = await _userService.CreateUserAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<UserResponseDto>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _userService.GetUserByIdAsync(id, ct);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail("کاربر یافت نشد."));

        return Ok(ApiResponse<UserResponseDto>.Ok(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool? isActive = null,
        CancellationToken ct = default)
    {
        var result = await _userService.GetUsersAsync(new GetUsersQuery(pageNumber, pageSize, isActive), ct);
        return Ok(ApiResponse<PagedResult<UserResponseDto>>.Ok(result));
    }
}
