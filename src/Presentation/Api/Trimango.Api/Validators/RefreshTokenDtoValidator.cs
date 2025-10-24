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
            Required(x => x.AccessToken, GetLocalizedMessage("Auth.AccessToken"));

            Required(x => x.RefreshToken, GetLocalizedMessage("Auth.RefreshToken"));
        }
    }
}
