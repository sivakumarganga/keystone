using FluentValidation;
using KeyStone.Shared.API.RequestModels;

namespace KeyStone.Shared.API.Validators;

public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(x=>x.UserName)
            .NotNull()
            .NotEmpty()
            .WithMessage("Username is required");
        RuleFor(x=>x.Password)
            .NotNull()
            .NotEmpty()
            .WithMessage("Password is required");
        RuleFor(x=>x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Password is required");
        RuleFor(x=>x.PhoneNumber)
            .NotNull()
            .NotEmpty()
            .WithMessage("Password is required");
    }
}