using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;
using System.Data;

namespace Trimango.Mssql.Migrations
{
    [MaggsoftMigration("2025/01/19 14:10:00", "Create CMS and User Interaction Tables - Page, Blog, Review, Question, Favorite, Message, Dispute, Coupon, Notification, SystemLog", "v05")]
    public sealed class CreateCmsAndUserInteractionTables : Migration
    {
        public override void Up()
        {
            // ========================================
            // CMS TABLOLARI
            // ========================================
            
            // Pages tablosu
            if (!Schema.Table("Pages").Exists())
            {
                Create.Table("Pages")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Title").AsString(200).NotNullable()
                    .WithColumn("Slug").AsString(200).NotNullable()
                    .WithColumn("Content").AsString().NotNullable()
                    .WithColumn("MetaTitle").AsString(200).NotNullable()
                    .WithColumn("MetaDescription").AsString(500).NotNullable()
                    .WithColumn("MetaKeywords").AsString(500).NotNullable()
                    .WithColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0)
                    .WithColumn("Template").AsString(100).NotNullable().WithDefaultValue("Default")
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_Pages_Slug").OnTable("Pages").OnColumn("Slug").Ascending().WithOptions().Unique();
            }
            
            // SeoContents tablosu
            if (!Schema.Table("SeoContents").Exists())
            {
                Create.Table("SeoContents")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Key").AsString(100).NotNullable()
                    .WithColumn("Title").AsString(200).NotNullable()
                    .WithColumn("Content").AsString().NotNullable()
                    .WithColumn("LanguageId").AsGuid().NotNullable()
                    .WithColumn("Section").AsInt32().NotNullable() // Enum stored as int
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_SeoContents_Key").OnTable("SeoContents").OnColumn("Key").Ascending();
                Create.Index("IX_SeoContents_Section").OnTable("SeoContents").OnColumn("Section").Ascending();
                Create.Index("IX_SeoContents_LanguageId").OnTable("SeoContents").OnColumn("LanguageId").Ascending();
            }
            
            // BlogCategories tablosu
            if (!Schema.Table("BlogCategories").Exists())
            {
                Create.Table("BlogCategories")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Name").AsString(100).NotNullable()
                    .WithColumn("Slug").AsString(100).NotNullable()
                    .WithColumn("Description").AsString(500).NotNullable()
                    .WithColumn("Color").AsString(7).NotNullable().WithDefaultValue("#007bff")
                    .WithColumn("IconUrl").AsString(500).NotNullable()
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_BlogCategories_Slug").OnTable("BlogCategories").OnColumn("Slug").Ascending().WithOptions().Unique();
            }
            
            // BlogPosts tablosu
            if (!Schema.Table("BlogPosts").Exists())
            {
                Create.Table("BlogPosts")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Title").AsString(200).NotNullable()
                    .WithColumn("Slug").AsString(200).NotNullable()
                    .WithColumn("Summary").AsString(1000).NotNullable()
                    .WithColumn("Content").AsString().NotNullable()
                    .WithColumn("FeaturedImageUrl").AsString(500).NotNullable()
                    .WithColumn("MetaTitle").AsString(200).NotNullable()
                    .WithColumn("MetaDescription").AsString(500).NotNullable()
                    .WithColumn("MetaKeywords").AsString(500).NotNullable()
                    .WithColumn("PublishedDate").AsDateTime().Nullable()
                    .WithColumn("ViewCount").AsInt32().NotNullable().WithDefaultValue(0)
                    .WithColumn("LikeCount").AsInt32().NotNullable().WithDefaultValue(0)
                    .WithColumn("CategoryId").AsGuid().NotNullable()
                    .WithColumn("AuthorId").AsGuid().NotNullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_BlogPosts_Slug").OnTable("BlogPosts").OnColumn("Slug").Ascending().WithOptions().Unique();
                Create.Index("IX_BlogPosts_CategoryId").OnTable("BlogPosts").OnColumn("CategoryId").Ascending();
                Create.Index("IX_BlogPosts_AuthorId").OnTable("BlogPosts").OnColumn("AuthorId").Ascending();
            }
            
            // BlogComments tablosu
            if (!Schema.Table("BlogComments").Exists())
            {
                Create.Table("BlogComments")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Name").AsString(100).NotNullable()
                    .WithColumn("Email").AsString(256).NotNullable()
                    .WithColumn("Content").AsString(1000).NotNullable()
                    .WithColumn("IsApproved").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("IsSpam").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("ParentCommentId").AsGuid().Nullable()
                    .WithColumn("BlogPostId").AsGuid().NotNullable()
                    .WithColumn("UserId").AsGuid().Nullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_BlogComments_BlogPostId").OnTable("BlogComments").OnColumn("BlogPostId").Ascending();
                Create.Index("IX_BlogComments_UserId").OnTable("BlogComments").OnColumn("UserId").Ascending();
                Create.Index("IX_BlogComments_ParentCommentId").OnTable("BlogComments").OnColumn("ParentCommentId").Ascending();
            }
            
            // BlogTags tablosu
            if (!Schema.Table("BlogTags").Exists())
            {
                Create.Table("BlogTags")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Name").AsString(100).NotNullable()
                    .WithColumn("Slug").AsString(100).NotNullable()
                    .WithColumn("Color").AsString(7).NotNullable().WithDefaultValue("#6c757d")
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_BlogTags_Slug").OnTable("BlogTags").OnColumn("Slug").Ascending().WithOptions().Unique();
            }
            
            // PageComments tablosu
            if (!Schema.Table("PageComments").Exists())
            {
                Create.Table("PageComments")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Name").AsString(100).NotNullable()
                    .WithColumn("Email").AsString(256).NotNullable()
                    .WithColumn("Content").AsString(1000).NotNullable()
                    .WithColumn("IsApproved").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("IsSpam").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("ParentCommentId").AsGuid().Nullable()
                    .WithColumn("PageId").AsGuid().NotNullable()
                    .WithColumn("UserId").AsGuid().Nullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_PageComments_PageId").OnTable("PageComments").OnColumn("PageId").Ascending();
                Create.Index("IX_PageComments_UserId").OnTable("PageComments").OnColumn("UserId").Ascending();
                Create.Index("IX_PageComments_ParentCommentId").OnTable("PageComments").OnColumn("ParentCommentId").Ascending();
            }
            
            // ========================================
            // KULLANICI ETKİLEŞİM TABLOLARI
            // ========================================
            
            // Reviews tablosu
            if (!Schema.Table("Reviews").Exists())
            {
                Create.Table("Reviews")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Title").AsString(200).NotNullable()
                    .WithColumn("Content").AsString(1000).NotNullable()
                    .WithColumn("Rating").AsInt32().NotNullable()
                    .WithColumn("IsApproved").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("IsVerified").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("PropertyId").AsGuid().NotNullable()
                    .WithColumn("UserId").AsGuid().NotNullable()
                    .WithColumn("ReservationId").AsGuid().Nullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_Reviews_PropertyId").OnTable("Reviews").OnColumn("PropertyId").Ascending();
                Create.Index("IX_Reviews_UserId").OnTable("Reviews").OnColumn("UserId").Ascending();
                Create.Index("IX_Reviews_Rating").OnTable("Reviews").OnColumn("Rating").Ascending();
            }
            
            // Questions tablosu
            if (!Schema.Table("Questions").Exists())
            {
                Create.Table("Questions")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("QuestionText").AsString(1000).NotNullable()
                    .WithColumn("AnswerText").AsString(1000).Nullable()
                    .WithColumn("IsAnswered").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("IsPublic").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("PropertyId").AsGuid().NotNullable()
                    .WithColumn("UserId").AsGuid().NotNullable()
                    .WithColumn("AnsweredByUserId").AsGuid().Nullable()
                    .WithColumn("AnsweredAt").AsDateTime().Nullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_Questions_PropertyId").OnTable("Questions").OnColumn("PropertyId").Ascending();
                Create.Index("IX_Questions_UserId").OnTable("Questions").OnColumn("UserId").Ascending();
                Create.Index("IX_Questions_IsAnswered").OnTable("Questions").OnColumn("IsAnswered").Ascending();
            }
            
            // Favorites tablosu
            if (!Schema.Table("Favorites").Exists())
            {
                Create.Table("Favorites")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("PropertyId").AsGuid().NotNullable()
                    .WithColumn("UserId").AsGuid().NotNullable()
                    .WithColumn("Notes").AsString(500).NotNullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_Favorites_PropertyId").OnTable("Favorites").OnColumn("PropertyId").Ascending();
                Create.Index("IX_Favorites_UserId").OnTable("Favorites").OnColumn("UserId").Ascending();
                Create.Index("IX_Favorites_User_Property").OnTable("Favorites").OnColumn("UserId").Ascending().OnColumn("PropertyId").Ascending().WithOptions().Unique();
            }
            
            // Messages tablosu
            if (!Schema.Table("Messages").Exists())
            {
                Create.Table("Messages")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Subject").AsString(200).NotNullable()
                    .WithColumn("Content").AsString(2000).NotNullable()
                    .WithColumn("IsRead").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("ReadAt").AsDateTime().Nullable()
                    .WithColumn("SenderId").AsGuid().NotNullable()
                    .WithColumn("ReceiverId").AsGuid().NotNullable()
                    .WithColumn("PropertyId").AsGuid().Nullable()
                    .WithColumn("ReservationId").AsGuid().Nullable()
                    .WithColumn("ParentMessageId").AsGuid().Nullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_Messages_SenderId").OnTable("Messages").OnColumn("SenderId").Ascending();
                Create.Index("IX_Messages_ReceiverId").OnTable("Messages").OnColumn("ReceiverId").Ascending();
                Create.Index("IX_Messages_PropertyId").OnTable("Messages").OnColumn("PropertyId").Ascending();
                Create.Index("IX_Messages_IsRead").OnTable("Messages").OnColumn("IsRead").Ascending();
            }
            
            // Disputes tablosu
            if (!Schema.Table("Disputes").Exists())
            {
                Create.Table("Disputes")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Title").AsString(200).NotNullable()
                    .WithColumn("Description").AsString(2000).NotNullable()
                    .WithColumn("Status").AsInt32().NotNullable() // Enum stored as int
                    .WithColumn("Priority").AsInt32().NotNullable() // Enum stored as int
                    .WithColumn("Resolution").AsString(2000).NotNullable()
                    .WithColumn("ResolvedAt").AsDateTime().Nullable()
                    .WithColumn("ReservationId").AsGuid().NotNullable()
                    .WithColumn("ComplainantId").AsGuid().NotNullable()
                    .WithColumn("AssignedToUserId").AsGuid().Nullable()
                    .WithColumn("ResolvedByUserId").AsGuid().Nullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_Disputes_ReservationId").OnTable("Disputes").OnColumn("ReservationId").Ascending();
                Create.Index("IX_Disputes_ComplainantId").OnTable("Disputes").OnColumn("ComplainantId").Ascending();
                Create.Index("IX_Disputes_Status").OnTable("Disputes").OnColumn("Status").Ascending();
            }
            
            // ========================================
            // KAMPANYA & OPERASYONEL TABLOLAR
            // ========================================
            
            // Coupons tablosu
            if (!Schema.Table("Coupons").Exists())
            {
                Create.Table("Coupons")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Code").AsString(50).NotNullable()
                    .WithColumn("Name").AsString(200).NotNullable()
                    .WithColumn("Description").AsString(500).NotNullable()
                    .WithColumn("Type").AsInt32().NotNullable() // Enum stored as int
                    .WithColumn("Value").AsDecimal(18,2).NotNullable()
                    .WithColumn("MinOrderAmount").AsDecimal(18,2).Nullable()
                    .WithColumn("MaxDiscountAmount").AsDecimal(18,2).Nullable()
                    .WithColumn("UsageLimit").AsInt32().NotNullable().WithDefaultValue(0)
                    .WithColumn("UsedCount").AsInt32().NotNullable().WithDefaultValue(0)
                    .WithColumn("StartDate").AsDateTime().NotNullable()
                    .WithColumn("EndDate").AsDateTime().NotNullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("ApplicableProperties").AsString().NotNullable()
                    .WithColumn("ApplicableUsers").AsString().NotNullable()
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_Coupons_Code").OnTable("Coupons").OnColumn("Code").Ascending().WithOptions().Unique();
                Create.Index("IX_Coupons_IsActive").OnTable("Coupons").OnColumn("IsActive").Ascending();
            }
            
            // CouponUsages tablosu
            if (!Schema.Table("CouponUsages").Exists())
            {
                Create.Table("CouponUsages")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("CouponId").AsGuid().NotNullable()
                    .WithColumn("UserId").AsGuid().NotNullable()
                    .WithColumn("ReservationId").AsGuid().NotNullable()
                    .WithColumn("DiscountAmount").AsDecimal(18,2).NotNullable()
                    .WithColumn("UsedAt").AsDateTime().NotNullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_CouponUsages_CouponId").OnTable("CouponUsages").OnColumn("CouponId").Ascending();
                Create.Index("IX_CouponUsages_UserId").OnTable("CouponUsages").OnColumn("UserId").Ascending();
                Create.Index("IX_CouponUsages_ReservationId").OnTable("CouponUsages").OnColumn("ReservationId").Ascending();
            }
            
            // Notifications tablosu
            if (!Schema.Table("Notifications").Exists())
            {
                Create.Table("Notifications")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Title").AsString(200).NotNullable()
                    .WithColumn("Message").AsString(1000).NotNullable()
                    .WithColumn("Type").AsInt32().NotNullable() // Enum stored as int
                    .WithColumn("IsRead").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("ReadAt").AsDateTime().Nullable()
                    .WithColumn("ActionUrl").AsString(500).NotNullable()
                    .WithColumn("IconUrl").AsString(500).NotNullable()
                    .WithColumn("UserId").AsGuid().NotNullable()
                    .WithColumn("RelatedEntityId").AsGuid().Nullable()
                    .WithColumn("RelatedEntityType").AsString(100).NotNullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_Notifications_UserId").OnTable("Notifications").OnColumn("UserId").Ascending();
                Create.Index("IX_Notifications_IsRead").OnTable("Notifications").OnColumn("IsRead").Ascending();
                Create.Index("IX_Notifications_Type").OnTable("Notifications").OnColumn("Type").Ascending();
            }
            
            // SystemLogs tablosu
            if (!Schema.Table("SystemLogs").Exists())
            {
                Create.Table("SystemLogs")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Level").AsInt32().NotNullable() // Enum stored as int
                    .WithColumn("Message").AsString(1000).NotNullable()
                    .WithColumn("Exception").AsString().Nullable()
                    .WithColumn("StackTrace").AsString().Nullable()
                    .WithColumn("Source").AsString(100).NotNullable()
                    .WithColumn("Action").AsString(100).NotNullable()
                    .WithColumn("UserAgent").AsString(500).Nullable()
                    .WithColumn("IPAddress").AsString(45).Nullable()
                    .WithColumn("UserId").AsGuid().Nullable()
                    .WithColumn("RequestId").AsString(100).Nullable()
                    .WithColumn("AdditionalData").AsString().Nullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_SystemLogs_Level").OnTable("SystemLogs").OnColumn("Level").Ascending();
                Create.Index("IX_SystemLogs_UserId").OnTable("SystemLogs").OnColumn("UserId").Ascending();
                Create.Index("IX_SystemLogs_Source").OnTable("SystemLogs").OnColumn("Source").Ascending();
                Create.Index("IX_SystemLogs_CreatedDate").OnTable("SystemLogs").OnColumn("CreatedDate").Ascending();
            }
        }

        public override void Down()
        {
            // Rollback işlemleri - tabloları sil
            if (Schema.Table("SystemLogs").Exists())
                Delete.Table("SystemLogs");
            
            if (Schema.Table("Notifications").Exists())
                Delete.Table("Notifications");
            
            if (Schema.Table("CouponUsages").Exists())
                Delete.Table("CouponUsages");
            
            if (Schema.Table("Coupons").Exists())
                Delete.Table("Coupons");
            
            if (Schema.Table("Disputes").Exists())
                Delete.Table("Disputes");
            
            if (Schema.Table("Messages").Exists())
                Delete.Table("Messages");
            
            if (Schema.Table("Favorites").Exists())
                Delete.Table("Favorites");
            
            if (Schema.Table("Questions").Exists())
                Delete.Table("Questions");
            
            if (Schema.Table("Reviews").Exists())
                Delete.Table("Reviews");
            
            if (Schema.Table("PageComments").Exists())
                Delete.Table("PageComments");
            
            if (Schema.Table("BlogTags").Exists())
                Delete.Table("BlogTags");
            
            if (Schema.Table("BlogComments").Exists())
                Delete.Table("BlogComments");
            
            if (Schema.Table("BlogPosts").Exists())
                Delete.Table("BlogPosts");
            
            if (Schema.Table("BlogCategories").Exists())
                Delete.Table("BlogCategories");
            
            if (Schema.Table("SeoContents").Exists())
                Delete.Table("SeoContents");
            
            if (Schema.Table("Pages").Exists())
                Delete.Table("Pages");
        }
    }
}
