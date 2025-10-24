using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Trimango.Data.Mssql.Entities;

namespace Trimango.Mssql
{
    public class TrimangoDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public TrimangoDbContext(DbContextOptions<TrimangoDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<PropertyFeatureMapping> PropertyFeatureMappings { get; set; }
        public DbSet<DistanceInfo> DistanceInfos { get; set; }
        public DbSet<SeasonalPricing> SeasonalPricings { get; set; }
        public DbSet<StayRule> StayRules { get; set; }
        public DbSet<ExtraFee> ExtraFees { get; set; }
        public DbSet<ReservationPriceBreakdown> ReservationPriceBreakdowns { get; set; }
        public DbSet<Policy> Policies { get; set; }
        
        // CMS Tabloları
        public DbSet<Page> Pages { get; set; }
        public DbSet<SeoContent> SeoContents { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<PageComment> PageComments { get; set; }
        
        // Kullanıcı Etkileşim Tabloları
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Dispute> Disputes { get; set; }
        
        // Kampanya & Operasyonel Tablolar
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CouponUsage> CouponUsages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }
        
        // Multilanguage Tabloları
        public DbSet<Language> Languages { get; set; }
        public DbSet<LocaleStringResource> LocaleStringResources { get; set; }
        public DbSet<LocalizedProperty> LocalizedProperties { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Global AsNoTracking aktif
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Global query filter (soft delete)
            modelBuilder.Entity<Supplier>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<Property>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<PropertyType>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<Location>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<Unit>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<PropertyImage>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<Reservation>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<Payment>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<Feature>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<PropertyFeatureMapping>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<DistanceInfo>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<SeasonalPricing>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<StayRule>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<ExtraFee>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<ReservationPriceBreakdown>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<Policy>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            
            // CMS Tabloları için global query filter
            modelBuilder.Entity<Page>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<SeoContent>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<BlogCategory>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<BlogPost>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<BlogComment>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<BlogTag>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<PageComment>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            
            // Kullanıcı Etkileşim Tabloları için global query filter
            modelBuilder.Entity<Review>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<Question>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<Favorite>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<Message>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            modelBuilder.Entity<Dispute>()
                .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
            
        // Kampanya & Operasyonel Tablolar için global query filter
        modelBuilder.Entity<Coupon>()
            .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
        modelBuilder.Entity<CouponUsage>()
            .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
        modelBuilder.Entity<Notification>()
            .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
        modelBuilder.Entity<SystemLog>()
            .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
        
        // Multilanguage Tabloları için global query filter
        modelBuilder.Entity<Language>()
            .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
        modelBuilder.Entity<LocaleStringResource>()
            .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
        modelBuilder.Entity<LocalizedProperty>()
            .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
        modelBuilder.Entity<City>()
            .HasQueryFilter(e => e.IsActive && !e.IsDeleted);
        modelBuilder.Entity<District>()
            .HasQueryFilter(e => e.IsActive && !e.IsDeleted);

            // Entity konfigürasyonları
            ConfigureEntities(modelBuilder);
        }

        private void ConfigureEntities(ModelBuilder modelBuilder)
        {
            // Supplier konfigürasyonu
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(150).IsRequired();
                entity.Property(e => e.TaxNumber).HasMaxLength(20).IsRequired();
                entity.Property(e => e.IBAN).HasMaxLength(34).IsRequired();
                entity.Property(e => e.ContractStatus).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Score).HasColumnType("decimal(18,2)");
            });

            // PropertyType konfigürasyonu
            modelBuilder.Entity<PropertyType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Slug).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.IconUrl).HasMaxLength(255);
            });

            // Location konfigürasyonu
            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CityId).IsRequired();
                entity.Property(e => e.DistrictId).IsRequired();
                entity.Property(e => e.Region).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Address).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Latitude).HasColumnType("decimal(10,8)");
                entity.Property(e => e.Longitude).HasColumnType("decimal(11,8)");
                
                // Foreign key ilişkileri
                entity.HasOne(e => e.City)
                    .WithMany(c => c.Locations)
                    .HasForeignKey(e => e.CityId)
                    .OnDelete(DeleteBehavior.NoAction);
                    
                entity.HasOne(e => e.District)
                    .WithMany(d => d.Locations)
                    .HasForeignKey(e => e.DistrictId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Property konfigürasyonu
            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(250).IsRequired();
                entity.Property(e => e.Description).HasColumnType("nvarchar(max)");
                
                entity.HasOne(e => e.Supplier)
                    .WithMany(s => s.Properties)
                    .HasForeignKey(e => e.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(e => e.PropertyType)
                    .WithMany(pt => pt.Properties)
                    .HasForeignKey(e => e.PropertyTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(e => e.Location)
                    .WithMany(l => l.Properties)
                    .HasForeignKey(e => e.LocationId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Unit konfigürasyonu
            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.BedConfig).HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)").IsRequired();
                
                entity.HasOne(e => e.Property)
                    .WithMany(p => p.Units)
                    .HasForeignKey(e => e.PropertyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // PropertyImage konfigürasyonu
            modelBuilder.Entity<PropertyImage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImageUrl).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Category).HasMaxLength(50).IsRequired();
                
                entity.HasOne(e => e.Property)
                    .WithMany(p => p.PropertyImages)
                    .HasForeignKey(e => e.PropertyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Reservation konfigürasyonu
            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Status).HasConversion<int>().IsRequired();
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)").IsRequired();
                
                entity.HasOne(e => e.Unit)
                    .WithMany(u => u.Reservations)
                    .HasForeignKey(e => e.UnitId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(e => e.Property)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(e => e.PropertyId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Reservations)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Payment konfigürasyonu
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Method).HasConversion<int>().IsRequired();
                entity.Property(e => e.Status).HasConversion<int>().IsRequired();
                entity.Property(e => e.TransactionRef).HasMaxLength(50);
                
                entity.HasOne(e => e.Reservation)
                    .WithMany(r => r.Payments)
                    .HasForeignKey(e => e.ReservationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Feature konfigürasyonu
            modelBuilder.Entity<Feature>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Category).HasConversion<int>().IsRequired();
                entity.Property(e => e.IconUrl).HasMaxLength(255);
            });

            // PropertyFeatureMapping konfigürasyonu
            modelBuilder.Entity<PropertyFeatureMapping>(entity =>
            {
                entity.HasKey(e => new { e.PropertyId, e.FeatureId });
                
                entity.HasOne(e => e.Property)
                    .WithMany(p => p.PropertyFeatureMappings)
                    .HasForeignKey(e => e.PropertyId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Feature)
                    .WithMany(f => f.PropertyFeatureMappings)
                    .HasForeignKey(e => e.FeatureId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // DistanceInfo konfigürasyonu
            modelBuilder.Entity<DistanceInfo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PlaceName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.PlaceType).HasConversion<int>().IsRequired();
                entity.Property(e => e.DistanceKm).HasColumnType("decimal(10,2)").IsRequired();
                
                entity.HasOne(e => e.Property)
                    .WithMany(p => p.DistanceInfos)
                    .HasForeignKey(e => e.PropertyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // SeasonalPricing konfigürasyonu
            modelBuilder.Entity<SeasonalPricing>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Scope).HasConversion<int>().IsRequired();
                entity.Property(e => e.PricePerNight).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Currency).HasMaxLength(10).IsRequired();
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                
                entity.HasOne(e => e.Property)
                    .WithMany(p => p.SeasonalPricings)
                    .HasForeignKey(e => e.ScopeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SeasonalPricing_Property");
                
                entity.HasOne(e => e.Unit)
                    .WithMany(u => u.SeasonalPricings)
                    .HasForeignKey(e => e.ScopeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SeasonalPricing_Unit");
            });

            // StayRule konfigürasyonu
            modelBuilder.Entity<StayRule>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Scope).HasConversion<int>().IsRequired();
                entity.Property(e => e.CheckInDays).HasMaxLength(50).IsRequired();
                
                entity.HasOne(e => e.Property)
                    .WithMany(p => p.StayRules)
                    .HasForeignKey(e => e.ScopeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_StayRule_Property");
                
                entity.HasOne(e => e.Unit)
                    .WithMany(u => u.StayRules)
                    .HasForeignKey(e => e.ScopeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_StayRule_Unit");
            });

            // ExtraFee konfigürasyonu
            modelBuilder.Entity<ExtraFee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Scope).HasConversion<int>().IsRequired();
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Type).HasConversion<int>().IsRequired();
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                
                entity.HasOne(e => e.Property)
                    .WithMany(p => p.ExtraFees)
                    .HasForeignKey(e => e.ScopeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ExtraFee_Property");
                
                entity.HasOne(e => e.Unit)
                    .WithMany(u => u.ExtraFees)
                    .HasForeignKey(e => e.ScopeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ExtraFee_Unit");
            });

            // ReservationPriceBreakdown konfigürasyonu
            modelBuilder.Entity<ReservationPriceBreakdown>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.LineType).HasConversion<int>().IsRequired();
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Total).HasColumnType("decimal(18,2)").IsRequired();
                
                entity.HasOne(e => e.Reservation)
                    .WithMany(r => r.PriceBreakdowns)
                    .HasForeignKey(e => e.ReservationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Policy konfigürasyonu
            modelBuilder.Entity<Policy>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PolicyType).HasConversion<int>().IsRequired();
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Terms).HasColumnType("nvarchar(max)");
            });

            // Reservation Policy ilişkisi
            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasOne(e => e.Policy)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(e => e.PolicyId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // CMS Entity'leri için enum konfigürasyonları
            modelBuilder.Entity<SeoContent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Key).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Section).HasConversion<int>().IsRequired();
                entity.Property(e => e.LanguageId).IsRequired();
            });

            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Type).HasConversion<int>().IsRequired();
                entity.Property(e => e.Value).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.MinOrderAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MaxDiscountAmount).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<CouponUsage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18,2)").IsRequired();
                
                entity.HasOne(e => e.Coupon)
                    .WithMany(c => c.CouponUsages)
                    .HasForeignKey(e => e.CouponId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Reservation)
                    .WithMany(r => r.CouponUsages)
                    .HasForeignKey(e => e.ReservationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Type).HasConversion<int>().IsRequired();
            });

            modelBuilder.Entity<SystemLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Level).HasConversion<int>().IsRequired();
                entity.Property(e => e.Message).HasMaxLength(1000).IsRequired();
                entity.Property(e => e.Source).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Action).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Dispute>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Status).HasConversion<int>().IsRequired();
                entity.Property(e => e.Priority).HasConversion<int>().IsRequired();
            });

            // Multilanguage Entity'leri için konfigürasyonlar
            modelBuilder.Entity<Language>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.LanguageCulture).HasMaxLength(10).IsRequired();
                entity.Property(e => e.UniqueSeoCode).HasMaxLength(10).IsRequired();
                entity.Property(e => e.FlagImageFileName).HasMaxLength(255);
                entity.Property(e => e.State).HasConversion<int>().IsRequired();
            });

            modelBuilder.Entity<LocaleStringResource>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ResourceName).HasMaxLength(200).IsRequired();
                entity.Property(e => e.ResourceValue).HasColumnType("nvarchar(max)").IsRequired();
                
                entity.HasOne(e => e.Language)
                    .WithMany(l => l.LocaleStringResources)
                    .HasForeignKey(e => e.LanguageId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LocalizedProperty>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.LocaleKey).HasMaxLength(100).IsRequired();
                entity.Property(e => e.LocaleValue).HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(e => e.LocaleKeyGroup).HasConversion<int>().IsRequired();
                
                entity.HasOne(e => e.Language)
                    .WithMany(l => l.LocalizedProperties)
                    .HasForeignKey(e => e.LanguageId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                
                entity.HasOne(e => e.City)
                    .WithMany(c => c.Districts)
                    .HasForeignKey(e => e.CityId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Property-City ve Property-District ilişkileri
            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasOne(e => e.City)
                    .WithMany(c => c.Properties)
                    .HasForeignKey(e => e.CityId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.District)
                    .WithMany(d => d.Properties)
                    .HasForeignKey(e => e.DistrictId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // ApplicationUser konfigürasyonu
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.ProfilePictureUrl).HasMaxLength(500);
                entity.Property(e => e.UpdatedIP).HasMaxLength(45);
                
                entity.HasOne(e => e.Supplier)
                    .WithMany(s => s.Users)
                    .HasForeignKey(e => e.SupplierId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
