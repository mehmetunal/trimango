namespace Trimango.Dto.Mssql.User
{
    /// <summary>
    /// Kullanıcı güncelleme DTO'su
    /// </summary>
    public class UpdateUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public bool IsSupplier { get; set; } = false;
    }
}
