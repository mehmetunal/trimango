using Microsoft.AspNetCore.Identity;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Uygulama rol entity'si (ASP.NET Core Identity)
    /// </summary>
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
