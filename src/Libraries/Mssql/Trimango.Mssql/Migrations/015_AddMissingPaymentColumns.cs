using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;
using Maggsoft.Data.Mssql;

namespace Trimango.Mssql.Migrations;

[MaggsoftMigration("2025/01/19 18:30:00", "Add missing Payment columns", "v15")]
public sealed class AddMissingPaymentColumns : Migration
{
    public override void Up()
    {
        if (Schema.Table("Payments").Exists())
        {
            // Currency kolonu ekle
            if (!Schema.Table("Payments").Column("Currency").Exists())
            {
                Alter.Table("Payments")
                    .AddColumn("Currency").AsString(3).NotNullable().WithDefaultValue("TRY");
            }
            
            // DisplayOrder kolonu ekle
            if (!Schema.Table("Payments").Column("DisplayOrder").Exists())
            {
                Alter.Table("Payments")
                    .AddColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0);
            }
            
            // PaidAt kolonu ekle
            if (!Schema.Table("Payments").Column("PaidAt").Exists())
            {
                Alter.Table("Payments")
                    .AddColumn("PaidAt").AsDateTime().Nullable();
            }
            
            // TransactionRef kolonu ekle (TransactionId yerine)
            if (!Schema.Table("Payments").Column("TransactionRef").Exists())
            {
                Alter.Table("Payments")
                    .AddColumn("TransactionRef").AsString(100).Nullable();
            }
            
            // CreatorIP kolonu ekle
            if (!Schema.Table("Payments").Column("CreatorIP").Exists())
            {
                Alter.Table("Payments")
                    .AddColumn("CreatorIP").AsString(45).Nullable();
            }
            
            // Status kolonunu int'e çevir (enum için)
            if (Schema.Table("Payments").Column("Status").Exists())
            {
                // Önce mevcut verileri temizle
                Execute.Sql("UPDATE [Payments] SET [Status] = '0' WHERE [Status] IS NOT NULL");
                
                Alter.Column("Status")
                    .OnTable("Payments")
                    .AsInt32()
                    .NotNullable();
            }
            
            // PaymentMethod kolonunu Method olarak yeniden adlandır ve int'e çevir
            if (Schema.Table("Payments").Column("PaymentMethod").Exists())
            {
                // Önce mevcut verileri temizle
                Execute.Sql("UPDATE [Payments] SET [PaymentMethod] = '0' WHERE [PaymentMethod] IS NOT NULL");
                
                // Kolonu yeniden adlandır ve int'e çevir
                Execute.Sql("EXEC sp_rename 'Payments.PaymentMethod', 'Method', 'COLUMN'");
                
                Alter.Column("Method")
                    .OnTable("Payments")
                    .AsInt32()
                    .NotNullable();
            }
        }
    }
    
    public override void Down()
    {
        // Rollback işlemleri (genellikle kullanılmaz)
    }
}
