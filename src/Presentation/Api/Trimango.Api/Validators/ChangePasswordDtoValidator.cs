using FluentValidation;
using Trimango.Api.Validators;
using Trimango.Dto.Mssql.Auth;
using Trimango.Mssql.Services.Interfaces;

namespace Trimango.Api.Validators
{
    /// <summary>
    /// ChangePasswordDto validator'ı
    /// </summary>
    public class ChangePasswordDtoValidator : LocalizedValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator(ILocalizationService localizationService) : base(localizationService)
        {
            Required(x => x.CurrentPassword, GetLocalizedMessage("User.CurrentPassword"));

            Required(x => x.NewPassword, GetLocalizedMessage("User.NewPassword"))
                .MinimumLength(6)
                .WithMessage(GetLocalizedMessage("Validation.MinLength", GetLocalizedMessage("User.NewPassword"), 6))
                .MaximumLength(100)
                .WithMessage(GetLocalizedMessage("Validation.MaxLength", GetLocalizedMessage("User.NewPassword"), 100))
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$")
                .WithMessage(GetLocalizedMessage("Validation.PasswordComplexity"));

            Required(x => x.ConfirmNewPassword, GetLocalizedMessage("User.ConfirmNewPassword"))
                .Equal(x => x.NewPassword)
                .WithMessage(GetLocalizedMessage("Validation.PasswordsDoNotMatch"));
        }
    }
}
