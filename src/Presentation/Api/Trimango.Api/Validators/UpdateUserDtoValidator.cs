using FluentValidation;
using Trimango.Api.Validators;
using Trimango.Dto.Mssql.User;
using Trimango.Mssql.Services.Interfaces;

namespace Trimango.Api.Validators
{
    /// <summary>
    /// UpdateUserDto validator'ı
    /// </summary>
    public class UpdateUserDtoValidator : LocalizedValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator(ILocalizationService localizationService) : base(localizationService)
        {
            Required(x => x.Email, GetLocalizedMessage("User.Email"))
                .EmailAddress()
                .WithMessage(GetLocalizedMessage("Validation.Email"))
                .MaximumLength(256)
                .WithMessage(GetLocalizedMessage("Validation.MaxLength", GetLocalizedMessage("User.Email"), 256));

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

            RuleFor(x => x.ProfilePictureUrl)
                .MaximumLength(500)
                .WithMessage(GetLocalizedMessage("Validation.MaxLength", GetLocalizedMessage("User.ProfilePictureUrl"), 500))
                .When(x => !string.IsNullOrEmpty(x.ProfilePictureUrl));
        }
    }
}
