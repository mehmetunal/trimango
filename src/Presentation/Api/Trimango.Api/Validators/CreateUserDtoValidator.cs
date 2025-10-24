using FluentValidation;
using Trimango.Dto.Mssql.User;

namespace Trimango.Api.Validators
{
    /// <summary>
    /// CreateUserDto validator'ı
    /// </summary>
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email adresi zorunludur")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz")
                .MaximumLength(256).WithMessage("Email adresi en fazla 256 karakter olabilir");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre zorunludur")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır")
                .MaximumLength(100).WithMessage("Şifre en fazla 100 karakter olabilir");

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
        }
    }
}
