using FluentValidation;

namespace EmployeeService.Application.DTOs;

public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
{
    public CreateEmployeeDtoValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Department);
        RuleFor(x => x.Position);
        RuleFor(x => x.EmploymentDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("تاریخ استخدام اشتباه می باشد.");
    }
}
