using Common;
using EmployeeService.Application.DTOs;
using EmployeeService.Application.Interfaces;
using EmployeeService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.Api.Controllers;

[ApiController]
[Route("api/reservations")]
public class EmployeesController(IEmployeeService reservationService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto, CancellationToken ct)
    {
        var result = await reservationService.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<EmployeeResponseDto>.Ok(result));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeDto dto, CancellationToken ct)
    {
        var result = await reservationService.UpdateAsync(id, dto, ct);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail("استخدام مورد نظر یافت نشد."));

        return Ok(ApiResponse<EmployeeResponseDto>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await reservationService.GetByIdAsync(id, ct);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail("رزرو یافت نشد."));

        return Ok(ApiResponse<EmployeeResponseDto>.Ok(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] int pageNumber = 1,
                                             [FromQuery] int pageSize = 10,
                                             [FromQuery] DateTime? fromDate = null,
                                             [FromQuery] DateTime? toDate = null,
                                             [FromQuery] EmployeeStatus? status = null,
                                             CancellationToken ct = default)
    {
        var query = new GetEmployeeQuery(pageNumber, pageSize, fromDate, toDate, status);
        var result = await reservationService.GetListAsync(query, ct);
        return Ok(ApiResponse<PagedResult<EmployeeResponseDto>>.Ok(result));
    }

    [HttpPatch("{id:guid}/preferences")]
    public async Task<IActionResult> UpdatePreferences(Guid id, [FromBody] EmployeePreferencesDto dto, CancellationToken ct)
    {
        var result = await reservationService.UpdatePreferencesAsync(id, dto, ct);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail("رزرو یافت نشد."));

        return Ok(ApiResponse<EmployeeResponseDto>.Ok(result));
    }
}
