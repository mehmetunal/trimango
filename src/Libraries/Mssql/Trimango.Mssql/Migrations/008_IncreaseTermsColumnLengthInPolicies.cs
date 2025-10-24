using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;
using Maggsoft.Data.Mssql;

namespace Trimango.Mssql.Migrations;

[MaggsoftMigration("2025/01/19 21:05:00", "Increase Terms column length in Policies table", "v08")]
public sealed class IncreaseTermsColumnLengthInPolicies : Migration
{
    public override void Up()
    {
        if (Schema.Table("Policies").Exists())
        {
            if (Schema.Table("Policies").Column("Terms").Exists())
            {
                Alter.Table("Policies")
                    .AlterColumn("Terms").AsString(1000).Nullable();
            }
        }
    }

    public override void Down()
    {
        if (Schema.Table("Policies").Exists())
        {
            if (Schema.Table("Policies").Column("Terms").Exists())
            {
                Alter.Table("Policies")
                    .AlterColumn("Terms").AsString(255).Nullable();
            }
        }
    }
}
