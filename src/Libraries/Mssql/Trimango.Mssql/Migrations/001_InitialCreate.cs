using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;
using Maggsoft.Data.Mssql;

namespace Trimango.Mssql.Migrations
{
    [MaggsoftMigration("2025/01/19 12:00:00", "Initial Create - Create all tables", "v01")]
    public sealed class InitialCreate : Migration
    {
        public override void Up()
        {
            // Identity Tables
            if (!Schema.Table("AspNetRoles").Exists())
            {
                Create.Table("AspNetRoles")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Name").AsString(256).Nullable()
                    .WithColumn("NormalizedName").AsString(256).Nullable()
                    .WithColumn("ConcurrencyStamp").AsString().Nullable();

                Create.Index("IX_AspNetRoles_NormalizedName")
                    .OnTable("AspNetRoles")
                    .OnColumn("NormalizedName")
                    .Ascending()
                    .WithOptions().Unique();
            }

            if (!Schema.Table("AspNetUsers").Exists())
            {
                Create.Table("AspNetUsers")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("UserName").AsString(256).Nullable()
                    .WithColumn("NormalizedUserName").AsString(256).Nullable()
                    .WithColumn("Email").AsString(256).Nullable()
                    .WithColumn("NormalizedEmail").AsString(256).Nullable()
                    .WithColumn("EmailConfirmed").AsBoolean().NotNullable()
                    .WithColumn("PasswordHash").AsString().Nullable()
                    .WithColumn("SecurityStamp").AsString().Nullable()
                    .WithColumn("ConcurrencyStamp").AsString().Nullable()
                    .WithColumn("PhoneNumber").AsString().Nullable()
                    .WithColumn("PhoneNumberConfirmed").AsBoolean().NotNullable()
                    .WithColumn("TwoFactorEnabled").AsBoolean().NotNullable()
                    .WithColumn("LockoutEnd").AsDateTimeOffset().Nullable()
                    .WithColumn("LockoutEnabled").AsBoolean().NotNullable()
                    .WithColumn("AccessFailedCount").AsInt32().NotNullable()
                    .WithColumn("FirstName").AsString(100).NotNullable()
                    .WithColumn("LastName").AsString(100).NotNullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("CreatorIP").AsString(45).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_AspNetUsers_NormalizedUserName")
                    .OnTable("AspNetUsers")
                    .OnColumn("NormalizedUserName")
                    .Ascending()
                    .WithOptions().Unique();

                Create.Index("IX_AspNetUsers_NormalizedEmail")
                    .OnTable("AspNetUsers")
                    .OnColumn("NormalizedEmail")
                    .Ascending();
            }

            if (!Schema.Table("AspNetUserRoles").Exists())
            {
                Create.Table("AspNetUserRoles")
                    .WithColumn("UserId").AsGuid().NotNullable()
                    .WithColumn("RoleId").AsGuid().NotNullable();

                Create.PrimaryKey("PK_AspNetUserRoles")
                    .OnTable("AspNetUserRoles")
                    .Columns("UserId", "RoleId");

                Create.ForeignKey("FK_AspNetUserRoles_AspNetRoles_RoleId")
                    .FromTable("AspNetUserRoles").ForeignColumn("RoleId")
                    .ToTable("AspNetRoles").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);

                Create.ForeignKey("FK_AspNetUserRoles_AspNetUsers_UserId")
                    .FromTable("AspNetUserRoles").ForeignColumn("UserId")
                    .ToTable("AspNetUsers").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);

                Create.Index("IX_AspNetUserRoles_RoleId")
                    .OnTable("AspNetUserRoles")
                    .OnColumn("RoleId")
                    .Ascending();
            }

            if (!Schema.Table("AspNetUserClaims").Exists())
            {
                Create.Table("AspNetUserClaims")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("UserId").AsGuid().NotNullable()
                    .WithColumn("ClaimType").AsString().Nullable()
                    .WithColumn("ClaimValue").AsString().Nullable();

                Create.ForeignKey("FK_AspNetUserClaims_AspNetUsers_UserId")
                    .FromTable("AspNetUserClaims").ForeignColumn("UserId")
                    .ToTable("AspNetUsers").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);

                Create.Index("IX_AspNetUserClaims_UserId")
                    .OnTable("AspNetUserClaims")
                    .OnColumn("UserId")
                    .Ascending();
            }

            if (!Schema.Table("AspNetUserLogins").Exists())
            {
                Create.Table("AspNetUserLogins")
                    .WithColumn("LoginProvider").AsString(128).NotNullable()
                    .WithColumn("ProviderKey").AsString(128).NotNullable()
                    .WithColumn("ProviderDisplayName").AsString().Nullable()
                    .WithColumn("UserId").AsGuid().NotNullable();

                Create.PrimaryKey("PK_AspNetUserLogins")
                    .OnTable("AspNetUserLogins")
                    .Columns("LoginProvider", "ProviderKey");

                Create.ForeignKey("FK_AspNetUserLogins_AspNetUsers_UserId")
                    .FromTable("AspNetUserLogins").ForeignColumn("UserId")
                    .ToTable("AspNetUsers").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);

                Create.Index("IX_AspNetUserLogins_UserId")
                    .OnTable("AspNetUserLogins")
                    .OnColumn("UserId")
                    .Ascending();
            }

            if (!Schema.Table("AspNetUserTokens").Exists())
            {
                Create.Table("AspNetUserTokens")
                    .WithColumn("UserId").AsGuid().NotNullable()
                    .WithColumn("LoginProvider").AsString(128).NotNullable()
                    .WithColumn("Name").AsString(128).NotNullable()
                    .WithColumn("Value").AsString().Nullable();

                Create.PrimaryKey("PK_AspNetUserTokens")
                    .OnTable("AspNetUserTokens")
                    .Columns("UserId", "LoginProvider", "Name");

                Create.ForeignKey("FK_AspNetUserTokens_AspNetUsers_UserId")
                    .FromTable("AspNetUserTokens").ForeignColumn("UserId")
                    .ToTable("AspNetUsers").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
            }

            if (!Schema.Table("AspNetRoleClaims").Exists())
            {
                Create.Table("AspNetRoleClaims")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("RoleId").AsGuid().NotNullable()
                    .WithColumn("ClaimType").AsString().Nullable()
                    .WithColumn("ClaimValue").AsString().Nullable();

                Create.ForeignKey("FK_AspNetRoleClaims_AspNetRoles_RoleId")
                    .FromTable("AspNetRoleClaims").ForeignColumn("RoleId")
                    .ToTable("AspNetRoles").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);

                Create.Index("IX_AspNetRoleClaims_RoleId")
                    .OnTable("AspNetRoleClaims")
                    .OnColumn("RoleId")
                    .Ascending();
            }

            // Business Tables
            if (!Schema.Table("PropertyTypes").Exists())
            {
                Create.Table("PropertyTypes")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Name").AsString(100).NotNullable()
                    .WithColumn("Slug").AsString(100).NotNullable()
                    .WithColumn("Description").AsString(500).Nullable()
                    .WithColumn("IconUrl").AsString(255).Nullable()
                    .WithColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0)
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("CreatorIP").AsString(45).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
            }

            if (!Schema.Table("Locations").Exists())
            {
                Create.Table("Locations")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("City").AsString(100).NotNullable()
                    .WithColumn("District").AsString(100).NotNullable()
                    .WithColumn("Region").AsString(100).NotNullable()
                    .WithColumn("Address").AsString(500).NotNullable()
                    .WithColumn("Latitude").AsDecimal(18,8).NotNullable()
                    .WithColumn("Longitude").AsDecimal(18,8).NotNullable()
                    .WithColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0)
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("CreatorIP").AsString(45).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
            }

            if (!Schema.Table("Suppliers").Exists())
            {
                Create.Table("Suppliers")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Name").AsString(200).NotNullable()
                    .WithColumn("TaxNumber").AsString(50).NotNullable()
                    .WithColumn("IBAN").AsString(50).NotNullable()
                    .WithColumn("ContractStatus").AsString(50).NotNullable()
                    .WithColumn("Score").AsDecimal(5,2).NotNullable().WithDefaultValue(0)
                    .WithColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0)
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("CreatorIP").AsString(45).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
            }

            if (!Schema.Table("Properties").Exists())
            {
                Create.Table("Properties")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Title").AsString(200).NotNullable()
                    .WithColumn("Description").AsString(2000).Nullable()
                    .WithColumn("PropertyTypeId").AsGuid().NotNullable()
                    .WithColumn("LocationId").AsGuid().NotNullable()
                    .WithColumn("SupplierId").AsGuid().NotNullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.ForeignKey("FK_Properties_PropertyTypes_PropertyTypeId")
                    .FromTable("Properties").ForeignColumn("PropertyTypeId")
                    .ToTable("PropertyTypes").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
                
                Create.ForeignKey("FK_Properties_Locations_LocationId")
                    .FromTable("Properties").ForeignColumn("LocationId")
                    .ToTable("Locations").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);

                Create.ForeignKey("FK_Properties_Suppliers_SupplierId")
                    .FromTable("Properties").ForeignColumn("SupplierId")
                    .ToTable("Suppliers").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
            }

            if (!Schema.Table("PropertyImages").Exists())
            {
                Create.Table("PropertyImages")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("PropertyId").AsGuid().NotNullable()
                    .WithColumn("ImageUrl").AsString(500).NotNullable()
                    .WithColumn("IsMain").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.ForeignKey("FK_PropertyImages_Properties_PropertyId")
                    .FromTable("PropertyImages").ForeignColumn("PropertyId")
                    .ToTable("Properties").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
            }

            if (!Schema.Table("Units").Exists())
            {
                Create.Table("Units")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("PropertyId").AsGuid().NotNullable()
                    .WithColumn("Name").AsString(100).NotNullable()
                    .WithColumn("Description").AsString(500).Nullable()
                    .WithColumn("Price").AsDecimal(18, 2).NotNullable()
                    .WithColumn("Capacity").AsInt32().NotNullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.ForeignKey("FK_Units_Properties_PropertyId")
                    .FromTable("Units").ForeignColumn("PropertyId")
                    .ToTable("Properties").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
            }

            if (!Schema.Table("Reservations").Exists())
            {
                Create.Table("Reservations")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("UnitId").AsGuid().NotNullable()
                    .WithColumn("PropertyId").AsGuid().Nullable()
                    .WithColumn("UserId").AsGuid().NotNullable()
                    .WithColumn("CheckInDate").AsDateTime().NotNullable()
                    .WithColumn("CheckOutDate").AsDateTime().NotNullable()
                    .WithColumn("GuestCount").AsInt32().NotNullable()
                    .WithColumn("TotalPrice").AsDecimal(18, 2).NotNullable()
                    .WithColumn("Currency").AsString(10).NotNullable().WithDefaultValue("TRY")
                    .WithColumn("Status").AsString(50).NotNullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("CreatorIP").AsString(45).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.ForeignKey("FK_Reservations_Units_UnitId")
                    .FromTable("Reservations").ForeignColumn("UnitId")
                    .ToTable("Units").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);

                Create.ForeignKey("FK_Reservations_Properties_PropertyId")
                    .FromTable("Reservations").ForeignColumn("PropertyId")
                    .ToTable("Properties").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.None);

                Create.ForeignKey("FK_Reservations_AspNetUsers_UserId")
                    .FromTable("Reservations").ForeignColumn("UserId")
                    .ToTable("AspNetUsers").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
            }

            if (!Schema.Table("Payments").Exists())
            {
                Create.Table("Payments")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("ReservationId").AsGuid().NotNullable()
                    .WithColumn("Amount").AsDecimal(18, 2).NotNullable()
                    .WithColumn("PaymentMethod").AsString(50).NotNullable()
                    .WithColumn("Status").AsString(50).NotNullable()
                    .WithColumn("TransactionId").AsString(100).Nullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.ForeignKey("FK_Payments_Reservations_ReservationId")
                    .FromTable("Payments").ForeignColumn("ReservationId")
                    .ToTable("Reservations").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
            }

            // Languages Table
            if (!Schema.Table("Languages").Exists())
            {
                Create.Table("Languages")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Name").AsString(100).NotNullable()
                    .WithColumn("LanguageCulture").AsString(10).NotNullable()
                    .WithColumn("UniqueSeoCode").AsString(10).NotNullable()
                    .WithColumn("FlagImageFileName").AsString(255).Nullable()
                    .WithColumn("Rtl").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("State").AsInt32().NotNullable()
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
            }

            // LocaleStringResources Table
            if (!Schema.Table("LocaleStringResources").Exists())
            {
                Create.Table("LocaleStringResources")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("LanguageId").AsGuid().NotNullable()
                    .WithColumn("ResourceName").AsString(500).NotNullable()
                    .WithColumn("ResourceValue").AsString(2000).NotNullable()
                    .WithColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0)
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("CreatorIP").AsString(45).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.ForeignKey("FK_LocaleStringResources_Languages_LanguageId")
                    .FromTable("LocaleStringResources").ForeignColumn("LanguageId")
                    .ToTable("Languages").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
                
                Create.Index("IX_LocaleStringResources_LanguageId")
                    .OnTable("LocaleStringResources")
                    .OnColumn("LanguageId")
                    .Ascending();
                
                Create.Index("IX_LocaleStringResources_ResourceName")
                    .OnTable("LocaleStringResources")
                    .OnColumn("ResourceName")
                    .Ascending();
            }

            // Cities Table
            if (!Schema.Table("Cities").Exists())
            {
                Create.Table("Cities")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Name").AsString(100).NotNullable()
                    .WithColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0)
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("CreatorIP").AsString(45).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
            }

            // Districts Table
            if (!Schema.Table("Districts").Exists())
            {
                Create.Table("Districts")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("CityId").AsGuid().NotNullable()
                    .WithColumn("Name").AsString(100).NotNullable()
                    .WithColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0)
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("CreatorIP").AsString(45).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.ForeignKey("FK_Districts_Cities_CityId")
                    .FromTable("Districts").ForeignColumn("CityId")
                    .ToTable("Cities").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
                
                Create.Index("IX_Districts_CityId")
                    .OnTable("Districts")
                    .OnColumn("CityId")
                    .Ascending();
            }

            // DistanceInfos Table
            if (!Schema.Table("DistanceInfos").Exists())
            {
                Create.Table("DistanceInfos")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("PropertyId").AsGuid().NotNullable()
                    .WithColumn("PlaceName").AsString(100).NotNullable()
                    .WithColumn("DistanceKm").AsDecimal(10, 2).NotNullable()
                    .WithColumn("PlaceType").AsInt32().NotNullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("CreatorIP").AsString(45).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable()
                    .WithColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0);
                
                Create.ForeignKey("FK_DistanceInfos_Properties_PropertyId")
                    .FromTable("DistanceInfos").ForeignColumn("PropertyId")
                    .ToTable("Properties").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
                
                Create.Index("IX_DistanceInfos_PropertyId")
                    .OnTable("DistanceInfos")
                    .OnColumn("PropertyId")
                    .Ascending();
                    
                Create.Index("IX_DistanceInfos_PlaceType")
                    .OnTable("DistanceInfos")
                    .OnColumn("PlaceType")
                    .Ascending();
            }

            // Features Table
            if (!Schema.Table("Features").Exists())
            {
                Create.Table("Features")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("Name").AsString(100).NotNullable()
                    .WithColumn("Category").AsInt32().NotNullable()
                    .WithColumn("IconUrl").AsString(255).Nullable()
                    .WithColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0)
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

            // PropertyFeatureMappings Table
            if (!Schema.Table("PropertyFeatureMappings").Exists())
            {
                Create.Table("PropertyFeatureMappings")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("PropertyId").AsGuid().NotNullable()
                    .WithColumn("FeatureId").AsGuid().NotNullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("CreatorIP").AsString(45).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable()
                    .WithColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0);
                
                Create.ForeignKey("FK_PropertyFeatureMappings_Properties_PropertyId")
                    .FromTable("PropertyFeatureMappings").ForeignColumn("PropertyId")
                    .ToTable("Properties").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
                
                Create.ForeignKey("FK_PropertyFeatureMappings_Features_FeatureId")
                    .FromTable("PropertyFeatureMappings").ForeignColumn("FeatureId")
                    .ToTable("Features").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
            }

            // SeasonalPricings Table
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
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("CreatorIP").AsString(45).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_SeasonalPricings_Scope_ScopeId")
                    .OnTable("SeasonalPricings")
                    .OnColumn("Scope")
                    .Ascending()
                    .OnColumn("ScopeId")
                    .Ascending();
            }

            // StayRules Table
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
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("CreatorIP").AsString(45).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_StayRules_Scope_ScopeId")
                    .OnTable("StayRules")
                    .OnColumn("Scope")
                    .Ascending()
                    .OnColumn("ScopeId")
                    .Ascending();
            }

            // ExtraFees Table
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
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("CreatorIP").AsString(45).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_ExtraFees_Scope_ScopeId")
                    .OnTable("ExtraFees")
                    .OnColumn("Scope")
                    .Ascending()
                    .OnColumn("ScopeId")
                    .Ascending();
            }

            // ReservationPriceBreakdowns Table
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
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("CreatorIP").AsString(45).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.ForeignKey("FK_ReservationPriceBreakdowns_Reservations_ReservationId")
                    .FromTable("ReservationPriceBreakdowns").ForeignColumn("ReservationId")
                    .ToTable("Reservations").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
            }

            // Policies Table
            if (!Schema.Table("Policies").Exists())
            {
                Create.Table("Policies")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("PolicyType").AsInt32().NotNullable()
                    .WithColumn("Name").AsString(100).NotNullable()
                    .WithColumn("Terms").AsString(255).Nullable()
                    .WithColumn("IsVisible").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("DisplayOrder").AsInt32().NotNullable().WithDefaultValue(0)
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsGuid().Nullable()
                    .WithColumn("CreatorIP").AsString(45).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsGuid().Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.Index("IX_Policies_PolicyType")
                    .OnTable("Policies")
                    .OnColumn("PolicyType")
                    .Ascending();
            }

        }

        public override void Down()
        {
            // Rollback genellikle kullanılmaz
        }
    }
}
