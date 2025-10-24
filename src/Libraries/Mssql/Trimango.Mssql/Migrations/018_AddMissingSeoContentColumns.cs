using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;
using Maggsoft.Data.Mssql;

namespace Trimango.Mssql.Migrations;

[MaggsoftMigration("2025/01/19 20:00:00", "Add missing SeoContent columns", "v18")]
public sealed class AddMissingSeoContentColumns : Migration
{
    public override void Up()
    {
        if (Schema.Table("SeoContents").Exists())
        {
            // CreatorIP kolonu ekle
            if (!Schema.Table("SeoContents").Column("CreatorIP").Exists())
            {
                Alter.Table("SeoContents")
                    .AddColumn("CreatorIP").AsString(45).Nullable();
            }
            
            // DisplayOrder kolonu ekle
            if (!Schema.Table("SeoContents").Column("DisplayOrder").Exists())
            {
                Alter.Table("SeoContents")
                    .AddColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0);
            }
            
            // IsActive kolonu ekle
            if (!Schema.Table("SeoContents").Column("IsActive").Exists())
            {
                Alter.Table("SeoContents")
                    .AddColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);
            }
        }
    }
    
    public override void Down()
    {
        if (Schema.Table("SeoContents").Exists())
        {
            if (Schema.Table("SeoContents").Column("CreatorIP").Exists())
            {
                Delete.Column("CreatorIP").FromTable("SeoContents");
            }
            
            if (Schema.Table("SeoContents").Column("DisplayOrder").Exists())
            {
                Delete.Column("DisplayOrder").FromTable("SeoContents");
            }
            
            if (Schema.Table("SeoContents").Column("IsActive").Exists())
            {
                Delete.Column("IsActive").FromTable("SeoContents");
            }
        }
    }
}
