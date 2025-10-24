using FluentValidation;
using FluentValidation.Validators;
using Trimango.Mssql.Services.Interfaces;

namespace Trimango.Api.Validators
{
    /// <summary>
    /// Localized validator base class
    /// </summary>
    public abstract class LocalizedValidator<T> : AbstractValidator<T>
    {
        protected readonly ILocalizationService _localizationService;

        protected LocalizedValidator(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        /// <summary>
        /// Localized error message oluşturur
        /// </summary>
        /// <param name="resourceName">Çeviri anahtarı</param>
        /// <returns>Localized error message</returns>
        protected string GetLocalizedMessage(string resourceName)
        {
            return _localizationService.GetResource(resourceName);
        }

        /// <summary>
        /// Localized error message oluşturur (parametreli)
        /// </summary>
        /// <param name="resourceName">Çeviri anahtarı</param>
        /// <param name="parameters">Parametreler</param>
        /// <returns>Localized error message</returns>
        protected string GetLocalizedMessage(string resourceName, params object[] parameters)
        {
            return _localizationService.GetResource(resourceName, parameters);
        }

        /// <summary>
        /// Required validation rule'u oluşturur
        /// </summary>
        /// <typeparam name="TProperty">Property tipi</typeparam>
        /// <param name="expression">Property expression</param>
        /// <param name="propertyName">Property adı (çeviri için)</param>
        /// <returns>Validation rule</returns>
        protected IRuleBuilderOptions<T, TProperty> Required<TProperty>(System.Linq.Expressions.Expression<Func<T, TProperty>> expression, string propertyName)
        {
            return RuleFor(expression)
                .NotEmpty()
                .WithMessage(GetLocalizedMessage("Validation.Required", propertyName));
        }

        /// <summary>
        /// Email validation rule'u oluşturur
        /// </summary>
        /// <param name="expression">Property expression</param>
        /// <returns>Validation rule</returns>
        protected IRuleBuilderOptions<T, string> Email(System.Linq.Expressions.Expression<Func<T, string>> expression)
        {
            return RuleFor(expression)
                .EmailAddress()
                .WithMessage(GetLocalizedMessage("Validation.Email"));
        }

        /// <summary>
        /// Minimum length validation rule'u oluşturur
        /// </summary>
        /// <param name="expression">Property expression</param>
        /// <param name="minLength">Minimum uzunluk</param>
        /// <param name="propertyName">Property adı (çeviri için)</param>
        /// <returns>Validation rule</returns>
        protected IRuleBuilderOptions<T, string> MinLength(System.Linq.Expressions.Expression<Func<T, string>> expression, int minLength, string propertyName)
        {
            return RuleFor(expression)
                .MinimumLength(minLength)
                .WithMessage(GetLocalizedMessage("Validation.MinLength", propertyName, minLength));
        }

        /// <summary>
        /// Maximum length validation rule'u oluşturur
        /// </summary>
        /// <param name="expression">Property expression</param>
        /// <param name="maxLength">Maximum uzunluk</param>
        /// <param name="propertyName">Property adı (çeviri için)</param>
        /// <returns>Validation rule</returns>
        protected IRuleBuilderOptions<T, string> MaxLength(System.Linq.Expressions.Expression<Func<T, string>> expression, int maxLength, string propertyName)
        {
            return RuleFor(expression)
                .MaximumLength(maxLength)
                .WithMessage(GetLocalizedMessage("Validation.MaxLength", propertyName, maxLength));
        }

        /// <summary>
        /// Phone number validation rule'u oluşturur
        /// </summary>
        /// <param name="expression">Property expression</param>
        /// <returns>Validation rule</returns>
        protected IRuleBuilderOptions<T, string> PhoneNumber(System.Linq.Expressions.Expression<Func<T, string>> expression)
        {
            return RuleFor(expression)
                .Matches(@"^[+]?[0-9\s\-\(\)]{10,}$")
                .WithMessage(GetLocalizedMessage("Validation.PhoneNumber"));
        }

        /// <summary>
        /// Password validation rule'u oluşturur
        /// </summary>
        /// <param name="expression">Property expression</param>
        /// <returns>Validation rule</returns>
        protected IRuleBuilderOptions<T, string> Password(System.Linq.Expressions.Expression<Func<T, string>> expression)
        {
            return RuleFor(expression)
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage(GetLocalizedMessage("Validation.Password"));
        }
    }
}
