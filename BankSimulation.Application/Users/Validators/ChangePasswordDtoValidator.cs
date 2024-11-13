using BankSimulation.Application.Extensions.Validators;
using BankSimulation.Application.Users.Dtos;
using FluentValidation;

namespace BankSimulation.Application.Users.Validators
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
