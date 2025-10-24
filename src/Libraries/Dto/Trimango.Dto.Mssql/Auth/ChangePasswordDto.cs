namespace Trimango.Dto.Mssql.Auth
{
    /// <summary>
    /// Şifre değiştirme DTO'su
    /// </summary>
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
