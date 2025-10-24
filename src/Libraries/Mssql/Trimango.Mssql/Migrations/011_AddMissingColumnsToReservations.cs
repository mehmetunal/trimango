using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;
using Maggsoft.Data.Mssql;

namespace Trimango.Mssql.Migrations;

[MaggsoftMigration("2025/01/19 16:00:00", "Add DisplayOrder column to Reservations table", "v11")]
public sealed class AddMissingColumnsToReservations : Migration
{
    public override void Up()
    {
        // Reservations tablosuna DisplayOrder kolonu ekle
        if (!Schema.Table("Reservations").Column("DisplayOrder").Exists())
        {
            Create.Column("DisplayOrder")
                .OnTable("Reservations")
                .AsInt32()
                .NotNullable()
                .WithDefaultValue(0);
        }
    }

    public override void Down()
    {
        // Rollback için kolonu kaldır
        if (Schema.Table("Reservations").Column("DisplayOrder").Exists())
        {
            Delete.Column("DisplayOrder").FromTable("Reservations");
        }
    }
}
