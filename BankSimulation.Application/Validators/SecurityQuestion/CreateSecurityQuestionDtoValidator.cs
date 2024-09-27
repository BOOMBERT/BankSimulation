using BankSimulation.Application.Dtos.SecurityQuestion;
using FluentValidation;

namespace BankSimulation.Application.Validators.SecurityQuestion
{
    public sealed class CreateSecurityQuestionDtoValidator : AbstractValidator<CreateSecurityQuestionDto>
    {
        public CreateSecurityQuestionDtoValidator()
        {
            RuleFor(u => u.Question)
                .NotEmpty().WithMessage("Question is required.")
                .MaximumLength(128).WithMessage("Question must be at most 128 characters long.");

            RuleFor(u => u.Answer)
                .NotEmpty().WithMessage("Answer is required.")
                .MaximumLength(256).WithMessage("Answer must be at most 256 characters long.");
        }
    }
}
