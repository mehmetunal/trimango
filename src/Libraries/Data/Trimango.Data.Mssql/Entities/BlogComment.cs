using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Blog yorumu entity'si
    /// </summary>
    public class BlogComment : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsApproved { get; set; } = false;
        public bool IsSpam { get; set; } = false;
        public Guid? ParentCommentId { get; set; }
        public Guid BlogPostId { get; set; }
        public Guid? UserId { get; set; } // Giriş yapmış kullanıcı
        
        // Navigation Properties
        public virtual BlogPost BlogPost { get; set; } = null!;
        public virtual ApplicationUser? User { get; set; }
        public virtual BlogComment? ParentComment { get; set; }
        public virtual ICollection<BlogComment> Replies { get; set; } = new List<BlogComment>();
    }
}
