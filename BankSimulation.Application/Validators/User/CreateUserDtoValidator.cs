using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Extensions.Validators;
using FluentValidation;

namespace BankSimulation.Application.Validators.User
{
    public sealed class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
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
