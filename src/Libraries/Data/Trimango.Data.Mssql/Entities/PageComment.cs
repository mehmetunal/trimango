using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Sayfa yorumu entity'si
    /// </summary>
    public class PageComment : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsApproved { get; set; } = false;
        public bool IsSpam { get; set; } = false;
        public Guid? ParentCommentId { get; set; }
        public Guid PageId { get; set; }
        public Guid? UserId { get; set; } // Giriş yapmış kullanıcı
        
        // Navigation Properties
        public virtual Page Page { get; set; } = null!;
        public virtual ApplicationUser? User { get; set; }
        public virtual PageComment? ParentComment { get; set; }
        public virtual ICollection<PageComment> Replies { get; set; } = new List<PageComment>();
    }
}
