using FluentValidation;
using Trimango.Api.Validators;
using Trimango.Dto.Mssql.Auth;
using Trimango.Mssql.Services.Interfaces;

namespace Trimango.Api.Validators
{
    /// <summary>
    /// RegisterDto validator'ı
    /// </summary>
    public class RegisterDtoValidator : LocalizedValidator<RegisterDto>
    {
        public RegisterDtoValidator(ILocalizationService localizationService) : base(localizationService)
        {
            Required(x => x.Email, GetLocalizedMessage("User.Email"))
                .EmailAddress()
                .WithMessage(GetLocalizedMessage("Validation.Email"))
                .MaximumLength(256)
                .WithMessage(GetLocalizedMessage("Validation.MaxLength", GetLocalizedMessage("User.Email"), 256));

            Required(x => x.Password, GetLocalizedMessage("User.Password"))
                .MinimumLength(6)
                .WithMessage(GetLocalizedMessage("Validation.MinLength", GetLocalizedMessage("User.Password"), 6))
                .MaximumLength(100)
                .WithMessage(GetLocalizedMessage("Validation.MaxLength", GetLocalizedMessage("User.Password"), 100))
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$")
                .WithMessage(GetLocalizedMessage("Validation.PasswordComplexity"));

            Required(x => x.ConfirmPassword, GetLocalizedMessage("User.ConfirmPassword"))
                .Equal(x => x.Password)
                .WithMessage(GetLocalizedMessage("Validation.PasswordsDoNotMatch"));

            Required(x => x.FirstName, GetLocalizedMessage("User.FirstName"))
                .MaximumLength(100)
                .WithMessage(GetLocalizedMessage("Validation.MaxLength", GetLocalizedMessage("User.FirstName"), 100));

            Required(x => x.LastName, GetLocalizedMessage("User.LastName"))
                .MaximumLength(100)
                .WithMessage(GetLocalizedMessage("Validation.MaxLength", GetLocalizedMessage("User.LastName"), 100));

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20)
                .WithMessage(GetLocalizedMessage("Validation.MaxLength", GetLocalizedMessage("User.PhoneNumber"), 20))
                .Matches(@"^[\+]?[1-9][\d]{0,15}$")
                .WithMessage(GetLocalizedMessage("Validation.PhoneNumber"))
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
        }
    }
}
