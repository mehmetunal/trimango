namespace Trimango.Dto.Mssql.Auth
{
    /// <summary>
    /// Kullanıcı giriş DTO'su
    /// </summary>
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; } = false;
    }
}
