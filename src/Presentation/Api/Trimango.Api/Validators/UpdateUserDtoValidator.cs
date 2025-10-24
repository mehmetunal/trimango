using FluentValidation;
using Trimango.Dto.Mssql.User;

namespace Trimango.Api.Validators
{
    /// <summary>
    /// UpdateUserDto validator'ı
    /// </summary>
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email adresi zorunludur")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz")
                .MaximumLength(256).WithMessage("Email adresi en fazla 256 karakter olabilir");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad zorunludur")
                .MaximumLength(100).WithMessage("Ad en fazla 100 karakter olabilir");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad zorunludur")
                .MaximumLength(100).WithMessage("Soyad en fazla 100 karakter olabilir");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).WithMessage("Telefon numarası en fazla 20 karakter olabilir")
                .Matches(@"^[\+]?[1-9][\d]{0,15}$").WithMessage("Geçerli bir telefon numarası giriniz")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            RuleFor(x => x.ProfilePictureUrl)
                .MaximumLength(500).WithMessage("Profil resmi URL'si en fazla 500 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.ProfilePictureUrl));
        }
    }
}
