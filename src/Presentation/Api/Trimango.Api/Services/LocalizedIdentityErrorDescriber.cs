using Microsoft.AspNetCore.Identity;
using Trimango.Mssql.Services.Interfaces;


/// <summary>
/// ASP.NET Core Identity için localized error describer
/// </summary>
public class LocalizedIdentityErrorDescriber(ILocalizationService localizationService) 
    : IdentityErrorDescriber
{
    private readonly ILocalizationService _localizationService = localizationService;

    public override IdentityError DefaultError()
    {
        return new IdentityError
        {
            Code = nameof(DefaultError),
            Description = _localizationService.GetResource("Identity.DefaultError")
        };
    }

    public override IdentityError ConcurrencyFailure()
    {
        return new IdentityError
        {
            Code = nameof(ConcurrencyFailure),
            Description = _localizationService.GetResource("Identity.ConcurrencyFailure")
        };
    }

    public override IdentityError PasswordMismatch()
    {
        return new IdentityError
        {
            Code = nameof(PasswordMismatch),
            Description = _localizationService.GetResource("Identity.PasswordMismatch")
        };
    }

    public override IdentityError InvalidToken()
    {
        return new IdentityError
        {
            Code = nameof(InvalidToken),
            Description = _localizationService.GetResource("Identity.InvalidToken")
        };
    }

    public override IdentityError LoginAlreadyAssociated()
    {
        return new IdentityError
        {
            Code = nameof(LoginAlreadyAssociated),
            Description = _localizationService.GetResource("Identity.LoginAlreadyAssociated")
        };
    }

    public override IdentityError InvalidUserName(string userName)
    {
        return new IdentityError
        {
            Code = nameof(InvalidUserName),
            Description = _localizationService.GetResource("Identity.InvalidUserName", [userName])
        };
    }

    public override IdentityError InvalidEmail(string email)
    {
        return new IdentityError
        {
            Code = nameof(InvalidEmail),
            Description = _localizationService.GetResource("Identity.InvalidEmail", [email])
        };
    }

    public override IdentityError DuplicateUserName(string userName)
    {
        return new IdentityError
        {
            Code = nameof(DuplicateUserName),
            Description = _localizationService.GetResource("Identity.DuplicateUserName", [userName])
        };
    }

    public override IdentityError DuplicateEmail(string email)
    {
        return new IdentityError
        {
            Code = nameof(DuplicateEmail),
            Description = _localizationService.GetResource("Identity.DuplicateEmail", [email])
        };
    }

    public override IdentityError InvalidRoleName(string role)
    {
        return new IdentityError
        {
            Code = nameof(InvalidRoleName),
            Description = _localizationService.GetResource("Identity.InvalidRoleName", [role])
        };
    }

    public override IdentityError DuplicateRoleName(string role)
    {
        return new IdentityError
        {
            Code = nameof(DuplicateRoleName),
            Description = _localizationService.GetResource("Identity.DuplicateRoleName", [role])
        };
    }

    public override IdentityError UserAlreadyHasPassword()
    {
        return new IdentityError
        {
            Code = nameof(UserAlreadyHasPassword),
            Description = _localizationService.GetResource("Identity.UserAlreadyHasPassword")
        };
    }

    public override IdentityError UserLockoutNotEnabled()
    {
        return new IdentityError
        {
            Code = nameof(UserLockoutNotEnabled),
            Description = _localizationService.GetResource("Identity.UserLockoutNotEnabled")
        };
    }

    public override IdentityError UserAlreadyInRole(string role)
    {
        return new IdentityError
        {
            Code = nameof(UserAlreadyInRole),
            Description = _localizationService.GetResource("Identity.UserAlreadyInRole", [role])
        };
    }

    public override IdentityError UserNotInRole(string role)
    {
        return new IdentityError
        {
            Code = nameof(UserNotInRole),
            Description = _localizationService.GetResource("Identity.UserNotInRole", [role])
        };
    }

    public override IdentityError PasswordTooShort(int length)
    {
        return new IdentityError
        {
            Code = nameof(PasswordTooShort),
            Description = _localizationService.GetResource("Identity.PasswordTooShort", [length])
        };
    }

    public override IdentityError PasswordRequiresNonAlphanumeric()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresNonAlphanumeric),
            Description = _localizationService.GetResource("Identity.PasswordRequiresNonAlphanumeric")
        };
    }

    public override IdentityError PasswordRequiresDigit()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresDigit),
            Description = _localizationService.GetResource("Identity.PasswordRequiresDigit")
        };
    }

    public override IdentityError PasswordRequiresLower()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresLower),
            Description = _localizationService.GetResource("Identity.PasswordRequiresLower")
        };
    }

    public override IdentityError PasswordRequiresUpper()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresUpper),
            Description = _localizationService.GetResource("Identity.PasswordRequiresUpper")
        };
    }

    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresUniqueChars),
            Description = _localizationService.GetResource("Identity.PasswordRequiresUniqueChars", [uniqueChars])
        };
    }

    public override IdentityError RecoveryCodeRedemptionFailed()
    {
        return new IdentityError
        {
            Code = nameof(RecoveryCodeRedemptionFailed),
            Description = _localizationService.GetResource("Identity.RecoveryCodeRedemptionFailed")
        };
    }
} 