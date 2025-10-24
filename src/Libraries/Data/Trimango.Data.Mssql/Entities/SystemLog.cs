using Trimango.Data.Mssql;
using Trimango.Data.Mssql.Enums;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Sistem log entity'si
    /// </summary>
    public class SystemLog : BaseEntity
    {
        public LogLevel Level { get; set; } = LogLevel.Information; // Information, Warning, Error, Critical
        public string Message { get; set; } = string.Empty;
        public string? Exception { get; set; }
        public string? StackTrace { get; set; }
        public string Source { get; set; } = string.Empty; // Controller, Service, Middleware vb.
        public string Action { get; set; } = string.Empty; // Create, Update, Delete, Login vb.
        public string? UserAgent { get; set; }
        public string? IPAddress { get; set; }
        public Guid? UserId { get; set; }
        public string? RequestId { get; set; }
        public string? AdditionalData { get; set; } // JSON formatında ek veriler
        
        // Navigation Properties
        public virtual ApplicationUser? User { get; set; }
    }
}
