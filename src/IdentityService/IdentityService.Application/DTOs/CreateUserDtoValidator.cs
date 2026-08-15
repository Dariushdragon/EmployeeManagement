using FluentValidation;

namespace IdentityService.Application.DTOs;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("FullName الزامی است.")
            .MaximumLength(200);

        RuleFor(x => x.Mobile)
            .NotEmpty().WithMessage("Mobile الزامی است.")
            .Matches(@"^09\d{9}$").WithMessage("فرمت شماره موبایل صحیح نیست.");
    }
}
