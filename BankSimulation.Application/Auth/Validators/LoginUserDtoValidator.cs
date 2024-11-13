using BankSimulation.Application.Auth.Dtos;
using BankSimulation.Application.Extensions.Validators;
using FluentValidation;

namespace BankSimulation.Application.Auth.Validators
{
    public sealed class LoginUserDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginUserDtoValidator()
        {
            RuleFor(u => u.Email)
                .IsValidEmail();

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
