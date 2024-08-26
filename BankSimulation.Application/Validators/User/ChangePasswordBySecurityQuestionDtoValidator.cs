using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Extensions.Validators;
using FluentValidation;

namespace BankSimulation.Application.Validators.User
{
    public sealed class ChangePasswordBySecurityQuestionDtoValidator : AbstractValidator<ChangePasswordBySecurityQuestionDto>
    {
        public ChangePasswordBySecurityQuestionDtoValidator()
        {
            RuleFor(u => u.Answer)
                .NotEmpty().WithMessage("Answer is required.")
                .MaximumLength(256).WithMessage("Answer must be at most 256 characters long.");

            RuleFor(u => u.NewPassword)
                .IsValidPassword();
        }
    }
}
