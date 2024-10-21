using FluentValidation;
using KeyStone.Shared.API.RequestModels;

namespace KeyStone.API.RequestValidators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x=>x.Username)
            .NotNull()
            .NotEmpty()
            .WithMessage("Username is required");
        RuleFor(x=>x.Password)
            .NotNull()
            .NotEmpty()
            .WithMessage("Password is required");
    }
}