using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;
using Maggsoft.Data.Mssql;

namespace Trimango.Mssql.Migrations;

[MaggsoftMigration("2025/01/19 19:30:00", "Fix Pages Content column to support large text", "v17")]
public sealed class FixPagesContentColumn : Migration
{
    public override void Up()
    {
        if (Schema.Table("Pages").Exists())
        {
            // Content kolonunu NVARCHAR(MAX) olarak değiştir
            if (Schema.Table("Pages").Column("Content").Exists())
            {
                Execute.Sql("ALTER TABLE [Pages] ALTER COLUMN [Content] NVARCHAR(MAX) NOT NULL");
            }
        }
    }
    
    public override void Down()
    {
        if (Schema.Table("Pages").Exists())
        {
            // Rollback - Content kolonunu eski haline döndür
            if (Schema.Table("Pages").Column("Content").Exists())
            {
                Execute.Sql("ALTER TABLE [Pages] ALTER COLUMN [Content] NVARCHAR(4000) NOT NULL");
            }
        }
    }
}
