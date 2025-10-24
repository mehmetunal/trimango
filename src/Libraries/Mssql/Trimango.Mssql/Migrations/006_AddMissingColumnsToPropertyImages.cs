using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;
using Maggsoft.Data.Mssql;
using System.Data;

namespace Trimango.Mssql.Migrations
{
    /// <summary>
    /// PropertyImages tablosuna eksik kolonları ekler
    /// </summary>
    [MaggsoftMigration("2025/01/24 22:45:00", "Add Missing Columns to PropertyImages Table", "v06")]
    public sealed class AddMissingColumnsToPropertyImages : Migration
    {
        public override void Up()
        {
            if (Schema.Table("PropertyImages").Exists())
            {
                // AltText kolonu
                if (!Schema.Table("PropertyImages").Column("AltText").Exists())
                {
                    Alter.Table("PropertyImages")
                        .AddColumn("AltText").AsString(255).Nullable();
                }

                // Category kolonu
                if (!Schema.Table("PropertyImages").Column("Category").Exists())
                {
                    Alter.Table("PropertyImages")
                        .AddColumn("Category").AsString(50).Nullable();
                }

                // CreatorIP kolonu
                if (!Schema.Table("PropertyImages").Column("CreatorIP").Exists())
                {
                    Alter.Table("PropertyImages")
                        .AddColumn("CreatorIP").AsString(45).Nullable();
                }

                // DisplayOrder kolonu
                if (!Schema.Table("PropertyImages").Column("DisplayOrder").Exists())
                {
                    Alter.Table("PropertyImages")
                        .AddColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0);
                }

                // Order kolonu
                if (!Schema.Table("PropertyImages").Column("Order").Exists())
                {
                    Alter.Table("PropertyImages")
                        .AddColumn("Order").AsInt32().NotNullable().WithDefaultValue(0);
                }

                // Index'leri ekle
                if (!Schema.Table("PropertyImages").Index("IX_PropertyImages_DisplayOrder").Exists())
                {
                    Create.Index("IX_PropertyImages_DisplayOrder")
                        .OnTable("PropertyImages")
                        .OnColumn("DisplayOrder")
                        .Ascending();
                }

                if (!Schema.Table("PropertyImages").Index("IX_PropertyImages_Order").Exists())
                {
                    Create.Index("IX_PropertyImages_Order")
                        .OnTable("PropertyImages")
                        .OnColumn("Order")
                        .Ascending();
                }

                if (!Schema.Table("PropertyImages").Index("IX_PropertyImages_Category").Exists())
                {
                    Create.Index("IX_PropertyImages_Category")
                        .OnTable("PropertyImages")
                        .OnColumn("Category")
                        .Ascending();
                }
            }
        }

        public override void Down()
        {
            if (Schema.Table("PropertyImages").Exists())
            {
                // Index'leri kaldır
                if (Schema.Table("PropertyImages").Index("IX_PropertyImages_DisplayOrder").Exists())
                {
                    Delete.Index("IX_PropertyImages_DisplayOrder").OnTable("PropertyImages");
                }

                if (Schema.Table("PropertyImages").Index("IX_PropertyImages_Order").Exists())
                {
                    Delete.Index("IX_PropertyImages_Order").OnTable("PropertyImages");
                }

                if (Schema.Table("PropertyImages").Index("IX_PropertyImages_Category").Exists())
                {
                    Delete.Index("IX_PropertyImages_Category").OnTable("PropertyImages");
                }

                // Kolonları kaldır
                if (Schema.Table("PropertyImages").Column("AltText").Exists())
                {
                    Delete.Column("AltText").FromTable("PropertyImages");
                }

                if (Schema.Table("PropertyImages").Column("Category").Exists())
                {
                    Delete.Column("Category").FromTable("PropertyImages");
                }

                if (Schema.Table("PropertyImages").Column("CreatorIP").Exists())
                {
                    Delete.Column("CreatorIP").FromTable("PropertyImages");
                }

                if (Schema.Table("PropertyImages").Column("DisplayOrder").Exists())
                {
                    Delete.Column("DisplayOrder").FromTable("PropertyImages");
                }

                if (Schema.Table("PropertyImages").Column("Order").Exists())
                {
                    Delete.Column("Order").FromTable("PropertyImages");
                }
            }
        }
    }
}
