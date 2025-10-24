using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;

namespace Trimango.Mssql.Migrations
{
    [MaggsoftMigration("2025/01/19 14:05:00", "Add Missing Columns to AspNetUsers - IsSupplier, LastLoginDate, ProfilePictureUrl, SupplierId", "v04")]
    public sealed class AddMissingColumnsToAspNetUsers : Migration
    {
        public override void Up()
        {
            // AspNetUsers tablosuna eksik kolonları ekle
            if (Schema.Table("AspNetUsers").Exists())
            {
                if (!Schema.Table("AspNetUsers").Column("IsSupplier").Exists())
                {
                    Alter.Table("AspNetUsers")
                        .AddColumn("IsSupplier").AsBoolean().NotNullable().WithDefaultValue(false);
                }
                
                if (!Schema.Table("AspNetUsers").Column("LastLoginDate").Exists())
                {
                    Alter.Table("AspNetUsers")
                        .AddColumn("LastLoginDate").AsDateTime().Nullable();
                }
                
                if (!Schema.Table("AspNetUsers").Column("ProfilePictureUrl").Exists())
                {
                    Alter.Table("AspNetUsers")
                        .AddColumn("ProfilePictureUrl").AsString(500).Nullable();
                }
                
                if (!Schema.Table("AspNetUsers").Column("SupplierId").Exists())
                {
                    Alter.Table("AspNetUsers")
                        .AddColumn("SupplierId").AsGuid().Nullable();
                }
                
                if (!Schema.Table("AspNetUsers").Column("CreatorUserId").Exists())
                {
                    Alter.Table("AspNetUsers")
                        .AddColumn("CreatorUserId").AsGuid().Nullable();
                }
                
                if (!Schema.Table("AspNetUsers").Column("IsDeleted").Exists())
                {
                    Alter.Table("AspNetUsers")
                        .AddColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false);
                }
                
                if (!Schema.Table("AspNetUsers").Column("UpdatedDate").Exists())
                {
                    Alter.Table("AspNetUsers")
                        .AddColumn("UpdatedDate").AsDateTime().Nullable();
                }
                
                if (!Schema.Table("AspNetUsers").Column("UpdatedIP").Exists())
                {
                    Alter.Table("AspNetUsers")
                        .AddColumn("UpdatedIP").AsString(45).Nullable();
                }
            }
        }

        public override void Down()
        {
            // Rollback işlemleri
            if (Schema.Table("AspNetUsers").Exists())
            {
                if (Schema.Table("AspNetUsers").Column("IsSupplier").Exists())
                {
                    Delete.Column("IsSupplier").FromTable("AspNetUsers");
                }
                
                if (Schema.Table("AspNetUsers").Column("LastLoginDate").Exists())
                {
                    Delete.Column("LastLoginDate").FromTable("AspNetUsers");
                }
                
                if (Schema.Table("AspNetUsers").Column("ProfilePictureUrl").Exists())
                {
                    Delete.Column("ProfilePictureUrl").FromTable("AspNetUsers");
                }
                
                if (Schema.Table("AspNetUsers").Column("SupplierId").Exists())
                {
                    Delete.Column("SupplierId").FromTable("AspNetUsers");
                }
                
                if (Schema.Table("AspNetUsers").Column("CreatorUserId").Exists())
                {
                    Delete.Column("CreatorUserId").FromTable("AspNetUsers");
                }
                
                if (Schema.Table("AspNetUsers").Column("IsDeleted").Exists())
                {
                    Delete.Column("IsDeleted").FromTable("AspNetUsers");
                }
                
                if (Schema.Table("AspNetUsers").Column("UpdatedDate").Exists())
                {
                    Delete.Column("UpdatedDate").FromTable("AspNetUsers");
                }
                
                if (Schema.Table("AspNetUsers").Column("UpdatedIP").Exists())
                {
                    Delete.Column("UpdatedIP").FromTable("AspNetUsers");
                }
            }
        }
    }
}
