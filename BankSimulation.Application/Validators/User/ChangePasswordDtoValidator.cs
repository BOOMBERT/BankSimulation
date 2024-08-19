using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Extensions.Validators;
using FluentValidation;

namespace BankSimulation.Application.Validators.User
{
    public sealed class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(u => u.CurrentPassword)
                .IsValidPassword();

            RuleFor(u => u.NewPassword)
                .IsValidPassword();
        }
    }
}
