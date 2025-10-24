using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;
using Maggsoft.Data.Mssql;

namespace Trimango.Mssql.Migrations;

[MaggsoftMigration("2025/01/19 17:00:00", "Add missing columns to AspNetUsers table", "v13")]
public sealed class AddMissingAspNetUsersColumns : Migration
{
    public override void Up()
    {
        // AspNetUsers tablosuna eksik kolonları ekle
        if (!Schema.Table("AspNetUsers").Column("ProfilePictureUrl").Exists())
        {
            Create.Column("ProfilePictureUrl")
                .OnTable("AspNetUsers")
                .AsString(500)
                .Nullable();
        }

        if (!Schema.Table("AspNetUsers").Column("IsSupplier").Exists())
        {
            Create.Column("IsSupplier")
                .OnTable("AspNetUsers")
                .AsBoolean()
                .NotNullable()
                .WithDefaultValue(false);
        }

        if (!Schema.Table("AspNetUsers").Column("SupplierId").Exists())
        {
            Create.Column("SupplierId")
                .OnTable("AspNetUsers")
                .AsGuid()
                .Nullable();
        }

        if (!Schema.Table("AspNetUsers").Column("LastLoginDate").Exists())
        {
            Create.Column("LastLoginDate")
                .OnTable("AspNetUsers")
                .AsDateTime()
                .Nullable();
        }

        // SupplierId için foreign key ekle
        if (!Schema.Table("AspNetUsers").Constraint("FK_AspNetUsers_Suppliers_SupplierId").Exists())
        {
            Create.ForeignKey("FK_AspNetUsers_Suppliers_SupplierId")
                .FromTable("AspNetUsers").ForeignColumn("SupplierId")
                .ToTable("Suppliers").PrimaryColumn("Id")
                .OnDelete(System.Data.Rule.SetNull);
        }

        // Index'ler ekle
        if (!Schema.Table("AspNetUsers").Index("IX_AspNetUsers_IsSupplier").Exists())
        {
            Create.Index("IX_AspNetUsers_IsSupplier")
                .OnTable("AspNetUsers")
                .OnColumn("IsSupplier")
                .Ascending();
        }

        if (!Schema.Table("AspNetUsers").Index("IX_AspNetUsers_SupplierId").Exists())
        {
            Create.Index("IX_AspNetUsers_SupplierId")
                .OnTable("AspNetUsers")
                .OnColumn("SupplierId")
                .Ascending();
        }
    }

    public override void Down()
    {
        // Foreign key'i kaldır
        if (Schema.Table("AspNetUsers").Constraint("FK_AspNetUsers_Suppliers_SupplierId").Exists())
        {
            Delete.ForeignKey("FK_AspNetUsers_Suppliers_SupplierId")
                .OnTable("AspNetUsers");
        }

        // Index'leri kaldır
        if (Schema.Table("AspNetUsers").Index("IX_AspNetUsers_IsSupplier").Exists())
        {
            Delete.Index("IX_AspNetUsers_IsSupplier")
                .OnTable("AspNetUsers");
        }

        if (Schema.Table("AspNetUsers").Index("IX_AspNetUsers_SupplierId").Exists())
        {
            Delete.Index("IX_AspNetUsers_SupplierId")
                .OnTable("AspNetUsers");
        }

        // Kolonları kaldır
        if (Schema.Table("AspNetUsers").Column("LastLoginDate").Exists())
        {
            Delete.Column("LastLoginDate").FromTable("AspNetUsers");
        }

        if (Schema.Table("AspNetUsers").Column("SupplierId").Exists())
        {
            Delete.Column("SupplierId").FromTable("AspNetUsers");
        }

        if (Schema.Table("AspNetUsers").Column("IsSupplier").Exists())
        {
            Delete.Column("IsSupplier").FromTable("AspNetUsers");
        }

        if (Schema.Table("AspNetUsers").Column("ProfilePictureUrl").Exists())
        {
            Delete.Column("ProfilePictureUrl").FromTable("AspNetUsers");
        }
    }
}
