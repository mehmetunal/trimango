using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;
using System.Data;

namespace Trimango.Mssql.Migrations
{
    [MaggsoftMigration("2025/01/24 18:00:00", "Create Multilanguage Tables - Language, LocaleStringResource, LocalizedProperty, City, District", "v06")]
    public sealed class CreateMultilanguageTables : Migration
    {
        public override void Up()
        {
            // Languages tablosu
            if (!Schema.Table("Languages").Exists())
            {
                Create.Table("Languages")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Name").AsString(100).NotNullable()
                    .WithColumn("LanguageCulture").AsString(10).NotNullable()
                    .WithColumn("UniqueSeoCode").AsString(10).NotNullable()
                    .WithColumn("FlagImageFileName").AsString(255).Nullable()
                    .WithColumn("Rtl").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("State").AsInt32().NotNullable() // Enum stored as int
                    .WithColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0)
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("CreatorIP").AsString(45).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();

                Create.Index("IX_Languages_UniqueSeoCode")
                    .OnTable("Languages")
                    .OnColumn("UniqueSeoCode")
                    .Ascending()
                    .WithOptions().Unique();

                Create.Index("IX_Languages_LanguageCulture")
                    .OnTable("Languages")
                    .OnColumn("LanguageCulture")
                    .Ascending();
            }

            // LocaleStringResources tablosu
            if (!Schema.Table("LocaleStringResources").Exists())
            {
                Create.Table("LocaleStringResources")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("LanguageId").AsGuid().NotNullable()
                    .WithColumn("ResourceName").AsString(200).NotNullable()
                    .WithColumn("ResourceValue").AsString().NotNullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();

                Create.Index("IX_LocaleStringResources_LanguageId")
                    .OnTable("LocaleStringResources")
                    .OnColumn("LanguageId")
                    .Ascending();

                Create.Index("IX_LocaleStringResources_ResourceName")
                    .OnTable("LocaleStringResources")
                    .OnColumn("ResourceName")
                    .Ascending();

                Create.Index("IX_LocaleStringResources_LanguageId_ResourceName")
                    .OnTable("LocaleStringResources")
                    .OnColumn("LanguageId")
                    .Ascending()
                    .OnColumn("ResourceName")
                    .Ascending()
                    .WithOptions().Unique();

                // Foreign Key
                Create.ForeignKey("FK_LocaleStringResources_Languages")
                    .FromTable("LocaleStringResources").ForeignColumn("LanguageId")
                    .ToTable("Languages").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
            }

            // LocalizedProperties tablosu
            if (!Schema.Table("LocalizedProperties").Exists())
            {
                Create.Table("LocalizedProperties")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("EntityId").AsGuid().NotNullable()
                    .WithColumn("LanguageId").AsGuid().NotNullable()
                    .WithColumn("LocaleKeyGroup").AsInt32().NotNullable() // Enum stored as int
                    .WithColumn("LocaleKey").AsString(100).NotNullable()
                    .WithColumn("LocaleValue").AsString().NotNullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();

                Create.Index("IX_LocalizedProperties_EntityId")
                    .OnTable("LocalizedProperties")
                    .OnColumn("EntityId")
                    .Ascending();

                Create.Index("IX_LocalizedProperties_LanguageId")
                    .OnTable("LocalizedProperties")
                    .OnColumn("LanguageId")
                    .Ascending();

                Create.Index("IX_LocalizedProperties_LocaleKeyGroup")
                    .OnTable("LocalizedProperties")
                    .OnColumn("LocaleKeyGroup")
                    .Ascending();

                // Foreign Key
                Create.ForeignKey("FK_LocalizedProperties_Languages")
                    .FromTable("LocalizedProperties").ForeignColumn("LanguageId")
                    .ToTable("Languages").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
            }

            // Cities tablosu
            if (!Schema.Table("Cities").Exists())
            {
                Create.Table("Cities")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Name").AsString(100).NotNullable()
                    .WithColumn("PlateCode").AsString(10).NotNullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();

                Create.Index("IX_Cities_Name")
                    .OnTable("Cities")
                    .OnColumn("Name")
                    .Ascending();

                Create.Index("IX_Cities_PlateCode")
                    .OnTable("Cities")
                    .OnColumn("PlateCode")
                    .Ascending()
                    .WithOptions().Unique();
            }

            // Districts tablosu
            if (!Schema.Table("Districts").Exists())
            {
                Create.Table("Districts")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Name").AsString(100).NotNullable()
                    .WithColumn("CityId").AsGuid().NotNullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();

                Create.Index("IX_Districts_Name")
                    .OnTable("Districts")
                    .OnColumn("Name")
                    .Ascending();

                Create.Index("IX_Districts_CityId")
                    .OnTable("Districts")
                    .OnColumn("CityId")
                    .Ascending();

                // Foreign Key
                Create.ForeignKey("FK_Districts_Cities")
                    .FromTable("Districts").ForeignColumn("CityId")
                    .ToTable("Cities").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
            }

            // Properties tablosuna CityId ve DistrictId kolonları ekle
            if (Schema.Table("Properties").Exists())
            {
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

                // Foreign Keys
                Create.ForeignKey("FK_Properties_Cities")
                    .FromTable("Properties").ForeignColumn("CityId")
                    .ToTable("Cities").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.None);

                Create.ForeignKey("FK_Properties_Districts")
                    .FromTable("Properties").ForeignColumn("DistrictId")
                    .ToTable("Districts").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.None);
            }

        }

        public override void Down()
        {
            // Foreign Key'leri kaldır
            if (Schema.Table("Properties").Exists())
            {
                if (Schema.Table("Properties").Constraint("FK_Properties_Cities").Exists())
                {
                    Delete.ForeignKey("FK_Properties_Cities").OnTable("Properties");
                }

                if (Schema.Table("Properties").Constraint("FK_Properties_Districts").Exists())
                {
                    Delete.ForeignKey("FK_Properties_Districts").OnTable("Properties");
                }

                // Kolonları kaldır
                if (Schema.Table("Properties").Column("CityId").Exists())
                {
                    Delete.Column("CityId").FromTable("Properties");
                }

                if (Schema.Table("Properties").Column("DistrictId").Exists())
                {
                    Delete.Column("DistrictId").FromTable("Properties");
                }
            }

            // Tabloları kaldır
            if (Schema.Table("Districts").Exists())
            {
                Delete.Table("Districts");
            }

            if (Schema.Table("Cities").Exists())
            {
                Delete.Table("Cities");
            }

            if (Schema.Table("LocalizedProperties").Exists())
            {
                Delete.Table("LocalizedProperties");
            }

            if (Schema.Table("LocaleStringResources").Exists())
            {
                Delete.Table("LocaleStringResources");
            }

            if (Schema.Table("Languages").Exists())
            {
                Delete.Table("Languages");
            }
        }
    }
}
