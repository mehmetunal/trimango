using Microsoft.AspNetCore.Identity;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Uygulama kullanıcı entity'si (ASP.NET Core Identity)
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public bool IsSupplier { get; set; } = false;
        public Guid? SupplierId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginDate { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Navigation Properties
        public virtual Supplier? Supplier { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
