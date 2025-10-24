namespace Trimango.Dto.Mssql.User
{
    /// <summary>
    /// Kullanıcı oluşturma DTO'su
    /// </summary>
    public class CreateUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public bool IsSupplier { get; set; } = false;
    }
}
