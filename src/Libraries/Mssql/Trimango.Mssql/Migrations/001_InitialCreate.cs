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
                    .WithColumn("Id").AsString(450).PrimaryKey()
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
                    .WithColumn("Id").AsString(450).PrimaryKey()
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
                    .WithColumn("CreatorUserId").AsString(450).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsString(450).Nullable()
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
                    .WithColumn("UserId").AsString(450).NotNullable()
                    .WithColumn("RoleId").AsString(450).NotNullable();
                
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
                    .WithColumn("UserId").AsString(450).NotNullable()
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
                    .WithColumn("UserId").AsString(450).NotNullable();
                
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
                    .WithColumn("UserId").AsString(450).NotNullable()
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
                    .WithColumn("RoleId").AsString(450).NotNullable()
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
                    .WithColumn("CreatorUserId").AsString(450).Nullable()
                    .WithColumn("CreatorIP").AsString(45).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsString(450).Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
            }

            if (!Schema.Table("Locations").Exists())
            {
                Create.Table("Locations")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("City").AsString(100).NotNullable()
                    .WithColumn("District").AsString(100).NotNullable()
                    .WithColumn("Country").AsString(100).NotNullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsString(450).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsString(450).Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
            }

            if (!Schema.Table("Suppliers").Exists())
            {
                Create.Table("Suppliers")
                    .WithColumn("Id").AsGuid().PrimaryKey()
                    .WithColumn("CompanyName").AsString(200).NotNullable()
                    .WithColumn("ContactPerson").AsString(100).NotNullable()
                    .WithColumn("Email").AsString(256).NotNullable()
                    .WithColumn("Phone").AsString(20).NotNullable()
                    .WithColumn("Address").AsString(500).Nullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsString(450).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsString(450).Nullable()
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
                    .WithColumn("CreatorUserId").AsString(450).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsString(450).Nullable()
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
                    .WithColumn("CreatorUserId").AsString(450).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsString(450).Nullable()
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
                    .WithColumn("CreatorUserId").AsString(450).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsString(450).Nullable()
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
                    .WithColumn("UserId").AsString(450).NotNullable()
                    .WithColumn("CheckInDate").AsDateTime().NotNullable()
                    .WithColumn("CheckOutDate").AsDateTime().NotNullable()
                    .WithColumn("TotalPrice").AsDecimal(18, 2).NotNullable()
                    .WithColumn("Status").AsString(50).NotNullable()
                    .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                    .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                    .WithColumn("CreatedDate").AsDateTime().NotNullable()
                    .WithColumn("CreatorUserId").AsString(450).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsString(450).Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.ForeignKey("FK_Reservations_Units_UnitId")
                    .FromTable("Reservations").ForeignColumn("UnitId")
                    .ToTable("Units").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
                
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
                    .WithColumn("CreatorUserId").AsString(450).Nullable()
                    .WithColumn("UpdatedDate").AsDateTime().Nullable()
                    .WithColumn("UpdatedByUserId").AsString(450).Nullable()
                    .WithColumn("UpdatedIP").AsString(45).Nullable();
                
                Create.ForeignKey("FK_Payments_Reservations_ReservationId")
                    .FromTable("Payments").ForeignColumn("ReservationId")
                    .ToTable("Reservations").PrimaryColumn("Id")
                    .OnDelete(System.Data.Rule.Cascade);
            }
        }

        public override void Down()
        {
            // Rollback genellikle kullanılmaz
        }
    }
}
