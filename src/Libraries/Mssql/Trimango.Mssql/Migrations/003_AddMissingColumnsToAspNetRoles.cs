using FluentMigrator;
using Maggsoft.Data.Migration.Attribute;

namespace Trimango.Mssql.Migrations
{
    [MaggsoftMigration("2025/01/19 14:00:00", "Add Missing Columns to AspNetRoles - Description, CreatedDate, IsActive", "v03")]
    public sealed class AddMissingColumnsToAspNetRoles : Migration
    {
        public override void Up()
        {
            // AspNetRoles tablosuna eksik kolonları ekle
            if (Schema.Table("AspNetRoles").Exists())
            {
                if (!Schema.Table("AspNetRoles").Column("Description").Exists())
                {
                    Alter.Table("AspNetRoles")
                        .AddColumn("Description").AsString(500).Nullable();
                }
                
                if (!Schema.Table("AspNetRoles").Column("CreatedDate").Exists())
                {
                    Alter.Table("AspNetRoles")
                        .AddColumn("CreatedDate").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime);
                }
                
                if (!Schema.Table("AspNetRoles").Column("IsActive").Exists())
                {
                    Alter.Table("AspNetRoles")
                        .AddColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);
                }
            }
        }

        public override void Down()
        {
            // Rollback işlemleri
            if (Schema.Table("AspNetRoles").Exists())
            {
                if (Schema.Table("AspNetRoles").Column("Description").Exists())
                {
                    Delete.Column("Description").FromTable("AspNetRoles");
                }
                
                if (Schema.Table("AspNetRoles").Column("CreatedDate").Exists())
                {
                    Delete.Column("CreatedDate").FromTable("AspNetRoles");
                }
                
                if (Schema.Table("AspNetRoles").Column("IsActive").Exists())
                {
                    Delete.Column("IsActive").FromTable("AspNetRoles");
                }
            }
        }
    }
}
