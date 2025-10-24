using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;
using Maggsoft.Data.Mssql;

namespace Trimango.Mssql.Migrations;

[MaggsoftMigration("2025/01/19 18:00:00", "Fix Reservation Status column type from string to int", "v14")]
public sealed class FixReservationStatusColumn : Migration
{
    public override void Up()
    {
        // Reservations tablosundaki Status kolonunu string'den int'e çevir
        if (Schema.Table("Reservations").Column("Status").Exists())
        {
            // Önce mevcut verileri temizle (test ortamı için)
            Execute.Sql("UPDATE [Reservations] SET [Status] = '0' WHERE [Status] IS NOT NULL");
            
            // Kolonu int olarak değiştir
            Alter.Column("Status")
                .OnTable("Reservations")
                .AsInt32()
                .NotNullable();
        }
    }

    public override void Down()
    {
        // Geri alma işlemi
        if (Schema.Table("Reservations").Column("Status").Exists())
        {
            Alter.Column("Status")
                .OnTable("Reservations")
                .AsString(50)
                .NotNullable();
        }
    }
}
