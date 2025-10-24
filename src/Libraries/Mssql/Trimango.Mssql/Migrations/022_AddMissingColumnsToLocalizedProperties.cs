using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;

namespace Trimango.Mssql.Migrations;

[MaggsoftMigration("2025/01/20 11:00:00", "Add missing CreatorIP and DisplayOrder columns to LocalizedProperties table", "v22")]
public sealed class AddMissingColumnsToLocalizedProperties : Migration
{
    public override void Up()
    {
        if (Schema.Table("LocalizedProperties").Exists())
        {
            if (!Schema.Table("LocalizedProperties").Column("CreatorIP").Exists())
            {
                Alter.Table("LocalizedProperties")
                    .AddColumn("CreatorIP").AsString(45).Nullable();
            }
            if (!Schema.Table("LocalizedProperties").Column("DisplayOrder").Exists())
            {
                Alter.Table("LocalizedProperties")
                    .AddColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0);
            }
        }
    }

    public override void Down()
    {
        if (Schema.Table("LocalizedProperties").Exists())
        {
            if (Schema.Table("LocalizedProperties").Column("CreatorIP").Exists())
            {
                Delete.Column("CreatorIP").FromTable("LocalizedProperties");
            }
            if (Schema.Table("LocalizedProperties").Column("DisplayOrder").Exists())
            {
                Delete.Column("DisplayOrder").FromTable("LocalizedProperties");
            }
        }
    }
}
