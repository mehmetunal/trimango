using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;
using Maggsoft.Data.Mssql;

namespace Trimango.Mssql.Migrations;

[MaggsoftMigration("2025/01/19 19:00:00", "Add CreatorIP column to Pages table", "v16")]
public sealed class AddCreatorIPToPages : Migration
{
    public override void Up()
    {
        if (Schema.Table("Pages").Exists())
        {
            // CreatorIP kolonu ekle
            if (!Schema.Table("Pages").Column("CreatorIP").Exists())
            {
                Alter.Table("Pages")
                    .AddColumn("CreatorIP").AsString(45).Nullable();
            }
        }
    }
    
    public override void Down()
    {
        if (Schema.Table("Pages").Exists())
        {
            if (Schema.Table("Pages").Column("CreatorIP").Exists())
            {
                Delete.Column("CreatorIP").FromTable("Pages");
            }
        }
    }
}
