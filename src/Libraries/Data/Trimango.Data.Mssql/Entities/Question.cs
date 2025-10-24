using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Soru-cevap entity'si
    /// </summary>
    public class Question : BaseEntity
    {
        public string QuestionText { get; set; } = string.Empty;
        public string? AnswerText { get; set; }
        public bool IsAnswered { get; set; } = false;
        public bool IsPublic { get; set; } = true;
        public Guid PropertyId { get; set; }
        public Guid UserId { get; set; }
        public Guid? AnsweredByUserId { get; set; } // Kim cevapladı
        public DateTime? AnsweredAt { get; set; }
        
        // Navigation Properties
        public virtual Property Property { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ApplicationUser? AnsweredByUser { get; set; }
    }
}
