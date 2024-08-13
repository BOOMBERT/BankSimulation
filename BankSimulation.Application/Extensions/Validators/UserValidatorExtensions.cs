using FluentValidation;

namespace BankSimulation.Application.Extensions.Validators
{
    internal static class UserValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> IsValidFirstName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(64).WithMessage("First name must be at most 64 characters long.");
        }

        public static IRuleBuilderOptions<T, string> IsValidLastName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(64).WithMessage("Last name must be at most 64 characters long.");
        }

        public static IRuleBuilderOptions<T, string> IsValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }

        public static IRuleBuilderOptions<T, string> IsValidPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(256).WithMessage("Password cannot exceed 256 characters.")
                .Matches(@"(?=.*\d)").WithMessage("Password must contain at least one digit.")
                .Matches(@"(?=.*[A-Z])").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"(?=.*[a-z])").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"(?=.*[\W_])").WithMessage("Password must contain at least one non-alphanumeric character.");
        }
    }
}
