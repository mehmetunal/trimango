using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;
using Maggsoft.Data.Mssql;

namespace Trimango.Mssql.Migrations
{
    /// <summary>
    /// Locations tablosunu günceller - City ve District string kolonlarını CityId ve DistrictId foreign key'leri ile değiştirir
    /// </summary>
    [MaggsoftMigration("2025/01/24 22:30:00", "Update Location Table - Replace City/District strings with foreign keys", "v04")]
    public sealed class UpdateLocationTable : Migration
    {
        public override void Up()
        {
            if (Schema.Table("Locations").Exists())
            {
                // Önce yeni kolonları ekle
                if (!Schema.Table("Locations").Column("CityId").Exists())
                {
                    Alter.Table("Locations")
                        .AddColumn("CityId").AsGuid().Nullable();
                }

                if (!Schema.Table("Locations").Column("DistrictId").Exists())
                {
                    Alter.Table("Locations")
                        .AddColumn("DistrictId").AsGuid().Nullable();
                }

                // Mevcut Locations tablosundaki verileri temizle (çünkü string'den Guid'e dönüştürme karmaşık)
                Delete.FromTable("Locations");

                // Eski kolonları sil
                if (Schema.Table("Locations").Column("City").Exists())
                {
                    Delete.Column("City").FromTable("Locations");
                }

                if (Schema.Table("Locations").Column("District").Exists())
                {
                    Delete.Column("District").FromTable("Locations");
                }

                // Yeni kolonları NOT NULL yap
                Alter.Table("Locations")
                    .AlterColumn("CityId").AsGuid().NotNullable();

                Alter.Table("Locations")
                    .AlterColumn("DistrictId").AsGuid().NotNullable();

                // Foreign key'leri ekle
                if (!Schema.Table("Locations").Constraint("FK_Locations_Cities_CityId").Exists())
                {
                    Create.ForeignKey("FK_Locations_Cities_CityId")
                        .FromTable("Locations").ForeignColumn("CityId")
                        .ToTable("Cities").PrimaryColumn("Id")
                        .OnDelete(System.Data.Rule.None);
                }

                if (!Schema.Table("Locations").Constraint("FK_Locations_Districts_DistrictId").Exists())
                {
                    Create.ForeignKey("FK_Locations_Districts_DistrictId")
                        .FromTable("Locations").ForeignColumn("DistrictId")
                        .ToTable("Districts").PrimaryColumn("Id")
                        .OnDelete(System.Data.Rule.None);
                }

                // Index'leri ekle
                if (!Schema.Table("Locations").Index("IX_Locations_CityId").Exists())
                {
                    Create.Index("IX_Locations_CityId")
                        .OnTable("Locations")
                        .OnColumn("CityId")
                        .Ascending();
                }

                if (!Schema.Table("Locations").Index("IX_Locations_DistrictId").Exists())
                {
                    Create.Index("IX_Locations_DistrictId")
                        .OnTable("Locations")
                        .OnColumn("DistrictId")
                        .Ascending();
                }
            }
        }

        public override void Down()
        {
            // Rollback genellikle kullanılmaz
        }
    }
}
