using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Extensions.Validators;
using FluentValidation;

namespace BankSimulation.Application.Validators.User
{
    public sealed class AdminUpdateUserDtoValidator : AbstractValidator<AdminUpdateUserDto>
    {
        public AdminUpdateUserDtoValidator()
        {
            RuleFor(u => u.FirstName)
                .IsValidFirstName();

            RuleFor(u => u.LastName)
                .IsValidLastName();

            RuleFor(u => u.Email)
                .IsValidEmail();

            RuleFor(u => u.Password)
                .IsValidPassword();
        }
    }
}
