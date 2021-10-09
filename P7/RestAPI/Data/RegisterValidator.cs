using FluentValidation;
using RestAPI.Models.Authentication;

namespace RestAPI.Data
{
    public class RegisterValidator : AbstractValidator<RegisterModel>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Please specify a UserName")
                .MinimumLength(3)
                .WithMessage("Minimum length for UserName is 3")
                .MaximumLength(50)
                .WithMessage("Maximum length for fullname is 50");

            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Please specify a full name")
                .MinimumLength(4)
                .WithMessage("Minimum length for fullname is 4")
                .MaximumLength(50)
                .WithMessage("Maximum length for fullname is 50");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Please specify an email")
                .EmailAddress()
                .WithMessage("Please specify a valid email");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Please specify a Password")
                .MinimumLength(8)
                .WithMessage("Minimum length for Password is 8");
        }
    }
}