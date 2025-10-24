using FluentValidation;
using Trimango.Api.Validators;
using Trimango.Dto.Mssql.Auth;
using Trimango.Mssql.Services.Interfaces;

namespace Trimango.Api.Validators
{
    /// <summary>
    /// RefreshTokenDto validator'ı
    /// </summary>
    public class RefreshTokenDtoValidator : LocalizedValidator<RefreshTokenDto>
    {
        public RefreshTokenDtoValidator(ILocalizationService localizationService) : base(localizationService)
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty()
                .WithMessage(GetLocalizedMessage("Validation.Required", GetLocalizedMessage("Auth.AccessToken")));

            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .WithMessage(GetLocalizedMessage("Validation.Required", GetLocalizedMessage("Auth.RefreshToken")));
        }
    }
}
