using FluentValidation;
using Trimango.Api.Validators;
using Trimango.Dto.Mssql.Auth;
using Trimango.Mssql.Services.Interfaces;

namespace Trimango.Api.Validators
{
    /// <summary>
    /// LoginDto validator'ı
    /// </summary>
    public class LoginDtoValidator : LocalizedValidator<LoginDto>
    {
        public LoginDtoValidator(ILocalizationService localizationService) : base(localizationService)
        {
            Required(x => x.Email, GetLocalizedMessage("User.Email"))
                .EmailAddress()
                .WithMessage(GetLocalizedMessage("Validation.Email"))
                .MaximumLength(256)
                .WithMessage(GetLocalizedMessage("Validation.MaxLength", GetLocalizedMessage("User.Email"), 256));

            Required(x => x.Password, GetLocalizedMessage("User.Password"))
                .MinimumLength(6)
                .WithMessage(GetLocalizedMessage("Validation.MinLength", GetLocalizedMessage("User.Password"), 6));
        }
    }
}
