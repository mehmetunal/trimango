using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Blog yazısı entity'si
    /// </summary>
    public class BlogPost : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string FeaturedImageUrl { get; set; } = string.Empty;
        public string MetaTitle { get; set; } = string.Empty;
        public string MetaDescription { get; set; } = string.Empty;
        public string MetaKeywords { get; set; } = string.Empty;
        public DateTime? PublishedDate { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public Guid CategoryId { get; set; }
        public Guid AuthorId { get; set; }
        
        // Navigation Properties
        public virtual BlogCategory Category { get; set; } = null!;
        public virtual ApplicationUser Author { get; set; } = null!;
        public virtual ICollection<BlogComment> Comments { get; set; } = new List<BlogComment>();
        public virtual ICollection<BlogTag> Tags { get; set; } = new List<BlogTag>();
    }
}
