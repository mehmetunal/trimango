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
                entity.Property(e => e.City).HasMaxLength(100).IsRequired();
                entity.Property(e => e.District).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Region).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Address).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Latitude).HasColumnType("decimal(10,8)");
                entity.Property(e => e.Longitude).HasColumnType("decimal(11,8)");
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
                entity.Property(e => e.Status).HasMaxLength(30).IsRequired();
                
                entity.HasOne(e => e.Unit)
                    .WithMany(u => u.Reservations)
                    .HasForeignKey(e => e.UnitId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(e => e.Property)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(e => e.PropertyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Payment konfigürasyonu
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Method).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
                entity.Property(e => e.TransactionRef).HasMaxLength(50);
                
                entity.HasOne(e => e.Reservation)
                    .WithMany(r => r.Payments)
                    .HasForeignKey(e => e.ReservationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
