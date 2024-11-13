using BankSimulation.Application.Extensions.Validators;
using BankSimulation.Application.Users.Dtos;
using FluentValidation;

namespace BankSimulation.Application.Users.Validators
{
    public sealed class ChangeEmailDtoValidator : AbstractValidator<ChangeEmailDto>
    {
        public ChangeEmailDtoValidator()
        {
            RuleFor(u => u.CurrentEmail)
                .IsValidEmail();

            RuleFor(u => u.NewEmail)
                .IsValidEmail();
        }
    }
}
