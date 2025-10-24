using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;
using Maggsoft.Data.Mssql;

namespace Trimango.Mssql.Migrations;

[MaggsoftMigration("2025/01/19 21:00:00", "Add DisplayOrder to SeasonalPricings, StayRules and ExtraFees", "v07")]
public sealed class AddDisplayOrderToSeasonalPricings : Migration
{
    public override void Up()
    {
        // SeasonalPricings Table
        if (Schema.Table("SeasonalPricings").Exists())
        {
            if (!Schema.Table("SeasonalPricings").Column("DisplayOrder").Exists())
            {
                Create.Column("DisplayOrder")
                    .OnTable("SeasonalPricings")
                    .AsInt32()
                    .NotNullable()
                    .WithDefaultValue(0);
            }
        }

        // StayRules Table
        if (Schema.Table("StayRules").Exists())
        {
            if (!Schema.Table("StayRules").Column("DisplayOrder").Exists())
            {
                Create.Column("DisplayOrder")
                    .OnTable("StayRules")
                    .AsInt32()
                    .NotNullable()
                    .WithDefaultValue(0);
            }
        }

        // ExtraFees Table
        if (Schema.Table("ExtraFees").Exists())
        {
            if (!Schema.Table("ExtraFees").Column("DisplayOrder").Exists())
            {
                Create.Column("DisplayOrder")
                    .OnTable("ExtraFees")
                    .AsInt32()
                    .NotNullable()
                    .WithDefaultValue(0);
            }
        }
    }

    public override void Down()
    {
        // SeasonalPricings Table
        if (Schema.Table("SeasonalPricings").Exists())
        {
            if (Schema.Table("SeasonalPricings").Column("DisplayOrder").Exists())
            {
                Delete.Column("DisplayOrder").FromTable("SeasonalPricings");
            }
        }

        // StayRules Table
        if (Schema.Table("StayRules").Exists())
        {
            if (Schema.Table("StayRules").Column("DisplayOrder").Exists())
            {
                Delete.Column("DisplayOrder").FromTable("StayRules");
            }
        }

        // ExtraFees Table
        if (Schema.Table("ExtraFees").Exists())
        {
            if (Schema.Table("ExtraFees").Column("DisplayOrder").Exists())
            {
                Delete.Column("DisplayOrder").FromTable("ExtraFees");
            }
        }
    }
}
