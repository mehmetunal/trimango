using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;
using System.Data;

namespace Trimango.Mssql.Migrations
{
    [MaggsoftMigration("2025/01/19 13:00:00", "Add New Entities - Feature, DistanceInfo, SeasonalPricing, StayRule, ExtraFee, ReservationPriceBreakdown, Policy", "v02")]
    public sealed class AddNewEntities : Migration
    {
        public override void Up()
        {
            // Features tablosu
            if (!Schema.Table("Features").Exists())
            {
                Create.Table("Features")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Name").AsString(100).NotNullable()
                    .WithColumn("Category").AsString(50).NotNullable()
                    .WithColumn("IconUrl").AsString(255).Nullable()
                    .WithColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0)
                    // Audit fields
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("CreatorIP").AsString(45).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_Features_Category")
                    .OnTable("Features")
                    .OnColumn("Category")
                    .Ascending();
            }

            // PropertyFeatureMappings tablosu
            if (!Schema.Table("PropertyFeatureMappings").Exists())
            {
                Create.Table("PropertyFeatureMappings")
                    .WithColumn("PropertyId").AsGuid().NotNullable()
                    .WithColumn("FeatureId").AsGuid().NotNullable()
                    // Audit fields
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.PrimaryKey("PK_PropertyFeatureMappings")
                    .OnTable("PropertyFeatureMappings")
                    .Columns("PropertyId", "FeatureId");
                
                Create.ForeignKey("FK_PropertyFeatureMappings_Properties_PropertyId")
                    .FromTable("PropertyFeatureMappings").ForeignColumn("PropertyId")
                    .ToTable("Properties").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
                
                Create.ForeignKey("FK_PropertyFeatureMappings_Features_FeatureId")
                    .FromTable("PropertyFeatureMappings").ForeignColumn("FeatureId")
                    .ToTable("Features").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
            }

            // DistanceInfos tablosu
            if (!Schema.Table("DistanceInfos").Exists())
            {
                Create.Table("DistanceInfos")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("PropertyId").AsGuid().NotNullable()
                    .WithColumn("PlaceName").AsString(100).NotNullable()
                    .WithColumn("DistanceKm").AsDecimal(10, 2).NotNullable()
                    .WithColumn("PlaceType").AsString(50).NotNullable()
                    // Audit fields
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.ForeignKey("FK_DistanceInfos_Properties_PropertyId")
                    .FromTable("DistanceInfos").ForeignColumn("PropertyId")
                    .ToTable("Properties").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
            }

            // SeasonalPricings tablosu
            if (!Schema.Table("SeasonalPricings").Exists())
            {
                Create.Table("SeasonalPricings")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Scope").AsString(50).NotNullable()
                    .WithColumn("ScopeId").AsGuid().NotNullable()
                    .WithColumn("StartDate").AsDateTime().NotNullable()
                    .WithColumn("EndDate").AsDateTime().NotNullable()
                    .WithColumn("PricePerNight").AsDecimal(18, 2).NotNullable()
                    .WithColumn("Currency").AsString(10).NotNullable().WithDefaultValue("TRY")
                    .WithColumn("Name").AsString(100).NotNullable()
                    // Audit fields
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_SeasonalPricings_Scope_ScopeId")
                    .OnTable("SeasonalPricings")
                    .OnColumn("Scope").Ascending()
                    .OnColumn("ScopeId").Ascending();
            }

            // StayRules tablosu
            if (!Schema.Table("StayRules").Exists())
            {
                Create.Table("StayRules")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Scope").AsString(50).NotNullable()
                    .WithColumn("ScopeId").AsGuid().NotNullable()
                    .WithColumn("MinNights").AsInt32().NotNullable()
                    .WithColumn("MaxNights").AsInt32().NotNullable()
                    .WithColumn("CheckInDays").AsString(50).NotNullable()
                    .WithColumn("GapNights").AsInt32().NotNullable().WithDefaultValue(0)
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    // Audit fields
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_StayRules_Scope_ScopeId")
                    .OnTable("StayRules")
                    .OnColumn("Scope").Ascending()
                    .OnColumn("ScopeId").Ascending();
            }

            // ExtraFees tablosu
            if (!Schema.Table("ExtraFees").Exists())
            {
                Create.Table("ExtraFees")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Scope").AsString(50).NotNullable()
                    .WithColumn("ScopeId").AsGuid().NotNullable()
                    .WithColumn("Name").AsString(100).NotNullable()
                    .WithColumn("Type").AsString(20).NotNullable()
                    .WithColumn("Amount").AsDecimal(18, 2).NotNullable()
                    .WithColumn("Mandatory").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("Description").AsString(500).Nullable()
                    // Audit fields
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_ExtraFees_Scope_ScopeId")
                    .OnTable("ExtraFees")
                    .OnColumn("Scope").Ascending()
                    .OnColumn("ScopeId").Ascending();
            }

            // ReservationPriceBreakdowns tablosu
            if (!Schema.Table("ReservationPriceBreakdowns").Exists())
            {
                Create.Table("ReservationPriceBreakdowns")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("ReservationId").AsGuid().NotNullable()
                    .WithColumn("LineType").AsString(50).NotNullable()
                    .WithColumn("Name").AsString(100).NotNullable()
                    .WithColumn("Qty").AsInt32().NotNullable()
                    .WithColumn("UnitPrice").AsDecimal(18, 2).NotNullable()
                    .WithColumn("Total").AsDecimal(18, 2).NotNullable()
                    .WithColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0)
                    // Audit fields
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.ForeignKey("FK_ReservationPriceBreakdowns_Reservations_ReservationId")
                    .FromTable("ReservationPriceBreakdowns").ForeignColumn("ReservationId")
                    .ToTable("Reservations").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
            }

            // Policies tablosu
            if (!Schema.Table("Policies").Exists())
            {
                Create.Table("Policies")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("PolicyType").AsString(20).NotNullable()
                    .WithColumn("Name").AsString(100).NotNullable()
                    .WithColumn("Terms").AsString().Nullable()
                    .WithColumn("IsVisible").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0)
                    // Audit fields
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_Policies_PolicyType")
                    .OnTable("Policies")
                    .OnColumn("PolicyType")
                    .Ascending();
            }

            // Reservations tablosuna yeni kolonlar ekle
            if (Schema.Table("Reservations").Exists())
            {
                if (!Schema.Table("Reservations").Column("GuestName").Exists())
                {
                    Alter.Table("Reservations")
                        .AddColumn("GuestName").AsString(100).NotNullable().WithDefaultValue("")
                        .AddColumn("GuestEmail").AsString(100).NotNullable().WithDefaultValue("")
                        .AddColumn("GuestPhone").AsString(20).NotNullable().WithDefaultValue("")
                        .AddColumn("SpecialRequests").AsString(500).Nullable()
                        .AddColumn("ConfirmedAt").AsDateTime().Nullable()
                        .AddColumn("CancelledAt").AsDateTime().Nullable()
                        .AddColumn("CancellationReason").AsString(500).Nullable();
                }

                // PolicyId foreign key ekle
                if (!Schema.Table("Reservations").Column("PolicyId").Exists())
                {
                    Alter.Table("Reservations")
                        .AddColumn("PolicyId").AsGuid().Nullable();
                    
                    Create.ForeignKey("FK_Reservations_Policies_PolicyId")
                        .FromTable("Reservations").ForeignColumn("PolicyId")
                        .ToTable("Policies").PrimaryColumn("Id")
                        .OnDelete(System.Data.Rule.SetNull);
                }
            }
        }

        public override void Down()
        {
            // Rollback işlemleri
            if (Schema.Table("Reservations").Exists())
            {
                if (Schema.Table("Reservations").Column("PolicyId").Exists())
                {
                    Delete.ForeignKey("FK_Reservations_Policies_PolicyId").OnTable("Reservations");
                    Delete.Column("PolicyId").FromTable("Reservations");
                }
                
                if (Schema.Table("Reservations").Column("GuestName").Exists())
                {
                    Delete.Column("GuestName").FromTable("Reservations");
                    Delete.Column("GuestEmail").FromTable("Reservations");
                    Delete.Column("GuestPhone").FromTable("Reservations");
                    Delete.Column("SpecialRequests").FromTable("Reservations");
                    Delete.Column("ConfirmedAt").FromTable("Reservations");
                    Delete.Column("CancelledAt").FromTable("Reservations");
                    Delete.Column("CancellationReason").FromTable("Reservations");
                }
            }

            if (Schema.Table("Policies").Exists())
                Delete.Table("Policies");
            
            if (Schema.Table("ReservationPriceBreakdowns").Exists())
                Delete.Table("ReservationPriceBreakdowns");
            
            if (Schema.Table("ExtraFees").Exists())
                Delete.Table("ExtraFees");
            
            if (Schema.Table("StayRules").Exists())
                Delete.Table("StayRules");
            
            if (Schema.Table("SeasonalPricings").Exists())
                Delete.Table("SeasonalPricings");
            
            if (Schema.Table("DistanceInfos").Exists())
                Delete.Table("DistanceInfos");
            
            if (Schema.Table("PropertyFeatureMappings").Exists())
                Delete.Table("PropertyFeatureMappings");
            
            if (Schema.Table("Features").Exists())
                Delete.Table("Features");
        }
    }
}
