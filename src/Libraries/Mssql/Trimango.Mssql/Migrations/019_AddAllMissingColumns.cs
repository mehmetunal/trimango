using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;
using Maggsoft.Data.Mssql;

namespace Trimango.Mssql.Migrations;

[MaggsoftMigration("2025/01/19 20:30:00", "Add all missing columns to all tables", "v19")]
public sealed class AddAllMissingColumns : Migration
{
    public override void Up()
    {
        // SeoContents tablosu için eksik kolonları ekle
        if (Schema.Table("SeoContents").Exists())
        {
            AddMissingColumnsToTable("SeoContents");
        }
        
        // BlogCategories tablosu için eksik kolonları ekle
        if (Schema.Table("BlogCategories").Exists())
        {
            AddMissingColumnsToTable("BlogCategories");
        }
        
        // BlogPosts tablosu için eksik kolonları ekle
        if (Schema.Table("BlogPosts").Exists())
        {
            AddMissingColumnsToTable("BlogPosts");
        }
        
        // BlogComments tablosu için eksik kolonları ekle
        if (Schema.Table("BlogComments").Exists())
        {
            AddMissingColumnsToTable("BlogComments");
        }
        
        // BlogTags tablosu için eksik kolonları ekle
        if (Schema.Table("BlogTags").Exists())
        {
            AddMissingColumnsToTable("BlogTags");
        }
        
        // PageComments tablosu için eksik kolonları ekle
        if (Schema.Table("PageComments").Exists())
        {
            AddMissingColumnsToTable("PageComments");
        }
        
        // Reviews tablosu için eksik kolonları ekle
        if (Schema.Table("Reviews").Exists())
        {
            AddMissingColumnsToTable("Reviews");
        }
        
        // Questions tablosu için eksik kolonları ekle
        if (Schema.Table("Questions").Exists())
        {
            AddMissingColumnsToTable("Questions");
        }
        
        // Favorites tablosu için eksik kolonları ekle
        if (Schema.Table("Favorites").Exists())
        {
            AddMissingColumnsToTable("Favorites");
        }
        
        // Messages tablosu için eksik kolonları ekle
        if (Schema.Table("Messages").Exists())
        {
            AddMissingColumnsToTable("Messages");
        }
        
        // Disputes tablosu için eksik kolonları ekle
        if (Schema.Table("Disputes").Exists())
        {
            AddMissingColumnsToTable("Disputes");
        }
        
        // Coupons tablosu için eksik kolonları ekle
        if (Schema.Table("Coupons").Exists())
        {
            AddMissingColumnsToTable("Coupons");
        }
        
        // CouponUsages tablosu için eksik kolonları ekle
        if (Schema.Table("CouponUsages").Exists())
        {
            AddMissingColumnsToTable("CouponUsages");
        }
        
        // Notifications tablosu için eksik kolonları ekle
        if (Schema.Table("Notifications").Exists())
        {
            AddMissingColumnsToTable("Notifications");
        }
        
        // SystemLogs tablosu için eksik kolonları ekle
        if (Schema.Table("SystemLogs").Exists())
        {
            AddMissingColumnsToTable("SystemLogs");
        }
        
        // LocaleStringResources tablosu için eksik kolonları ekle
        if (Schema.Table("LocaleStringResources").Exists())
        {
            AddMissingColumnsToTable("LocaleStringResources");
        }
        
        // LocalizedProperties tablosu için eksik kolonları ekle
        if (Schema.Table("LocalizedProperties").Exists())
        {
            AddMissingColumnsToTable("LocalizedProperties");
        }
    }
    
    private void AddMissingColumnsToTable(string tableName)
    {
        // CreatorIP kolonu ekle
        if (!Schema.Table(tableName).Column("CreatorIP").Exists())
        {
            Alter.Table(tableName)
                .AddColumn("CreatorIP").AsString(45).Nullable();
        }
        
        // DisplayOrder kolonu ekle
        if (!Schema.Table(tableName).Column("DisplayOrder").Exists())
        {
            Alter.Table(tableName)
                .AddColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0);
        }
        
        // IsActive kolonu ekle
        if (!Schema.Table(tableName).Column("IsActive").Exists())
        {
            Alter.Table(tableName)
                .AddColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);
        }
    }
    
    public override void Down()
    {
        // Rollback işlemleri (genellikle kullanılmaz)
    }
}
