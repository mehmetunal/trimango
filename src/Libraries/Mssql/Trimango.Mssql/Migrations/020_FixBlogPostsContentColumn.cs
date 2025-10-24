using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;
using Maggsoft.Data.Mssql;

namespace Trimango.Mssql.Migrations;

[MaggsoftMigration("2025/01/19 21:00:00", "Fix BlogPosts Content column to support large text", "v20")]
public sealed class FixBlogPostsContentColumn : Migration
{
    public override void Up()
    {
        if (Schema.Table("BlogPosts").Exists())
        {
            // Content kolonunu NVARCHAR(MAX) olarak değiştir
            if (Schema.Table("BlogPosts").Column("Content").Exists())
            {
                Execute.Sql("ALTER TABLE [BlogPosts] ALTER COLUMN [Content] NVARCHAR(MAX) NOT NULL");
            }
        }
    }
    
    public override void Down()
    {
        if (Schema.Table("BlogPosts").Exists())
        {
            // Rollback - Content kolonunu eski haline döndür
            if (Schema.Table("BlogPosts").Column("Content").Exists())
            {
                Execute.Sql("ALTER TABLE [BlogPosts] ALTER COLUMN [Content] NVARCHAR(4000) NOT NULL");
            }
        }
    }
}
