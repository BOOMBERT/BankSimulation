using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Extensions.Validators;
using FluentValidation;

namespace BankSimulation.Application.Validators.User
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
