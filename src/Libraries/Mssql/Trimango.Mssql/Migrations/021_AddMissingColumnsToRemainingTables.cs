using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;

namespace Trimango.Mssql.Migrations;

[MaggsoftMigration("2025/01/19 21:30:00", "Add missing columns to remaining tables that were missed", "v21")]
public sealed class AddMissingColumnsToRemainingTables : Migration
{
    public override void Up()
    {
        // LocalizedProperties tablosuna eksik kolonları ekle
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
            if (!Schema.Table("LocalizedProperties").Column("IsActive").Exists())
            {
                Alter.Table("LocalizedProperties")
                    .AddColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);
            }
        }

        // Diğer tabloları da kontrol et ve eksik kolonları ekle
        string[] additionalTables = new string[]
        {
            "BlogComments", "PageComments", "Reviews", "Questions", "Favorites", 
            "Disputes", "Coupons", "CouponUsages", "Notifications", "SystemLogs"
        };

        foreach (string table in additionalTables)
        {
            if (Schema.Table(table).Exists())
            {
                if (!Schema.Table(table).Column("CreatorIP").Exists())
                {
                    Alter.Table(table)
                        .AddColumn("CreatorIP").AsString(45).Nullable();
                }
                if (!Schema.Table(table).Column("DisplayOrder").Exists())
                {
                    Alter.Table(table)
                        .AddColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0);
                }
                if (!Schema.Table(table).Column("IsActive").Exists())
                {
                    Alter.Table(table)
                        .AddColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);
                }
            }
        }
    }

    public override void Down()
    {
        // LocalizedProperties tablosundan kolonları kaldır
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
            if (Schema.Table("LocalizedProperties").Column("IsActive").Exists())
            {
                Delete.Column("IsActive").FromTable("LocalizedProperties");
            }
        }

        // Diğer tablolardan da kolonları kaldır
        string[] additionalTables = new string[]
        {
            "BlogComments", "PageComments", "Reviews", "Questions", "Favorites", 
            "Disputes", "Coupons", "CouponUsages", "Notifications", "SystemLogs"
        };

        foreach (string table in additionalTables)
        {
            if (Schema.Table(table).Exists())
            {
                if (Schema.Table(table).Column("CreatorIP").Exists())
                {
                    Delete.Column("CreatorIP").FromTable(table);
                }
                if (Schema.Table(table).Column("DisplayOrder").Exists())
                {
                    Delete.Column("DisplayOrder").FromTable(table);
                }
                if (Schema.Table(table).Column("IsActive").Exists())
                {
                    Delete.Column("IsActive").FromTable(table);
                }
            }
        }
    }
}
