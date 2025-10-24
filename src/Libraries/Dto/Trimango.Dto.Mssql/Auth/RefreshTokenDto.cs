namespace Trimango.Dto.Mssql.Auth
{
    /// <summary>
    /// Token yenileme DTO'su
    /// </summary>
    public class RefreshTokenDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
