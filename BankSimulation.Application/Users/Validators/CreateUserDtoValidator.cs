using BankSimulation.Application.Extensions.Validators;
using BankSimulation.Application.Users.Dtos;
using FluentValidation;

namespace BankSimulation.Application.Users.Validators
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
