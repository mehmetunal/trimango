using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;
using Maggsoft.Data.Mssql;
using System.Data;

namespace Trimango.Mssql.Migrations
{
    /// <summary>
    /// Units tablosuna eksik kolonları ekler
    /// </summary>
    [MaggsoftMigration("2025/01/24 22:40:00", "Add Missing Columns to Units Table", "v05")]
    public sealed class AddMissingColumnsToUnits : Migration
    {
        public override void Up()
        {
            if (Schema.Table("Units").Exists())
            {
                // BedConfig kolonu
                if (!Schema.Table("Units").Column("BedConfig").Exists())
                {
                    Alter.Table("Units")
                        .AddColumn("BedConfig").AsString(500).Nullable();
                }

                // CreatorIP kolonu
                if (!Schema.Table("Units").Column("CreatorIP").Exists())
                {
                    Alter.Table("Units")
                        .AddColumn("CreatorIP").AsString(45).Nullable();
                }

                // Currency kolonu
                if (!Schema.Table("Units").Column("Currency").Exists())
                {
                    Alter.Table("Units")
                        .AddColumn("Currency").AsString(3).NotNullable().WithDefaultValue("TRY");
                }

                // DisplayOrder kolonu
                if (!Schema.Table("Units").Column("DisplayOrder").Exists())
                {
                    Alter.Table("Units")
                        .AddColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0);
                }

                // PrivatePool kolonu
                if (!Schema.Table("Units").Column("PrivatePool").Exists())
                {
                    Alter.Table("Units")
                        .AddColumn("PrivatePool").AsBoolean().NotNullable().WithDefaultValue(false);
                }

                // Index'leri ekle
                if (!Schema.Table("Units").Index("IX_Units_DisplayOrder").Exists())
                {
                    Create.Index("IX_Units_DisplayOrder")
                        .OnTable("Units")
                        .OnColumn("DisplayOrder")
                        .Ascending();
                }

                if (!Schema.Table("Units").Index("IX_Units_Currency").Exists())
                {
                    Create.Index("IX_Units_Currency")
                        .OnTable("Units")
                        .OnColumn("Currency")
                        .Ascending();
                }
            }
        }

        public override void Down()
        {
            if (Schema.Table("Units").Exists())
            {
                // Index'leri kaldır
                if (Schema.Table("Units").Index("IX_Units_DisplayOrder").Exists())
                {
                    Delete.Index("IX_Units_DisplayOrder").OnTable("Units");
                }

                if (Schema.Table("Units").Index("IX_Units_Currency").Exists())
                {
                    Delete.Index("IX_Units_Currency").OnTable("Units");
                }

                // Kolonları kaldır
                if (Schema.Table("Units").Column("BedConfig").Exists())
                {
                    Delete.Column("BedConfig").FromTable("Units");
                }

                if (Schema.Table("Units").Column("CreatorIP").Exists())
                {
                    Delete.Column("CreatorIP").FromTable("Units");
                }

                if (Schema.Table("Units").Column("Currency").Exists())
                {
                    Delete.Column("Currency").FromTable("Units");
                }

                if (Schema.Table("Units").Column("DisplayOrder").Exists())
                {
                    Delete.Column("DisplayOrder").FromTable("Units");
                }

                if (Schema.Table("Units").Column("PrivatePool").Exists())
                {
                    Delete.Column("PrivatePool").FromTable("Units");
                }
            }
        }
    }
}
