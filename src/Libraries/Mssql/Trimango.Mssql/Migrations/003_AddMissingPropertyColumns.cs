using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;
using Maggsoft.Data.Mssql;

namespace Trimango.Mssql.Migrations
{
    /// <summary>
    /// Properties tablosuna eksik kolonları ekler
    /// </summary>
    [MaggsoftMigration("2025/01/24 22:00:00", "Add Missing Property Columns", "v03")]
    public sealed class AddMissingPropertyColumns : Migration
    {
        public override void Up()
        {
            if (Schema.Table("Properties").Exists())
            {
                // Eksik kolonları ekle
                if (!Schema.Table("Properties").Column("Capacity").Exists())
                {
                    Alter.Table("Properties")
                        .AddColumn("Capacity").AsInt32().NotNullable().WithDefaultValue(1);
                }

                if (!Schema.Table("Properties").Column("RoomCount").Exists())
                {
                    Alter.Table("Properties")
                        .AddColumn("RoomCount").AsInt32().NotNullable().WithDefaultValue(1);
                }

                if (!Schema.Table("Properties").Column("BathroomCount").Exists())
                {
                    Alter.Table("Properties")
                        .AddColumn("BathroomCount").AsInt32().NotNullable().WithDefaultValue(1);
                }

                if (!Schema.Table("Properties").Column("SquareMeter").Exists())
                {
                    Alter.Table("Properties")
                        .AddColumn("SquareMeter").AsInt32().NotNullable().WithDefaultValue(50);
                }

                if (!Schema.Table("Properties").Column("CityId").Exists())
                {
                    Alter.Table("Properties")
                        .AddColumn("CityId").AsGuid().Nullable();
                }

                if (!Schema.Table("Properties").Column("DistrictId").Exists())
                {
                    Alter.Table("Properties")
                        .AddColumn("DistrictId").AsGuid().Nullable();
                }

                if (!Schema.Table("Properties").Column("CreatorIP").Exists())
                {
                    Alter.Table("Properties")
                        .AddColumn("CreatorIP").AsString(45).Nullable();
                }

                if (!Schema.Table("Properties").Column("DisplayOrder").Exists())
                {
                    Alter.Table("Properties")
                        .AddColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0);
                }

                // Foreign key'leri ekle
                if (!Schema.Table("Properties").Constraint("FK_Properties_Cities_CityId").Exists())
                {
                    Create.ForeignKey("FK_Properties_Cities_CityId")
                        .FromTable("Properties").ForeignColumn("CityId")
                        .ToTable("Cities").PrimaryColumn("Id")
                        .OnDelete(System.Data.Rule.None);
                }

                if (!Schema.Table("Properties").Constraint("FK_Properties_Districts_DistrictId").Exists())
                {
                    Create.ForeignKey("FK_Properties_Districts_DistrictId")
                        .FromTable("Properties").ForeignColumn("DistrictId")
                        .ToTable("Districts").PrimaryColumn("Id")
                        .OnDelete(System.Data.Rule.None);
                }

                // Index'leri ekle
                if (!Schema.Table("Properties").Index("IX_Properties_CityId").Exists())
                {
                    Create.Index("IX_Properties_CityId")
                        .OnTable("Properties")
                        .OnColumn("CityId")
                        .Ascending();
                }

                if (!Schema.Table("Properties").Index("IX_Properties_DistrictId").Exists())
                {
                    Create.Index("IX_Properties_DistrictId")
                        .OnTable("Properties")
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
