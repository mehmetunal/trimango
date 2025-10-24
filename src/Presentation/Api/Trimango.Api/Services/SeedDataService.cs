using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Trimango.Data.Mssql.Entities;
using Trimango.Data.Mssql.Enums;
using Trimango.Mssql;
using Bogus;
using Maggsoft.Core.Base;

namespace Trimango.Api.Services
{
    /// <summary>
    /// Seed data servisi - Test verilerini oluşturur
    /// </summary>
    public class SeedDataService
    {
        private readonly TrimangoDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<SeedDataService> _logger;

        public SeedDataService(
            TrimangoDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger<SeedDataService> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        /// <summary>
        /// Tüm seed data'yı oluşturur
        /// </summary>
        public async Task SeedAllDataAsync()
        {
            try
            {
                _logger.LogInformation("🌱 Seed data oluşturma başlatılıyor...");

                // 1. Rolleri oluştur
                await SeedRolesAsync();

                // 2. Admin kullanıcısını oluştur
                await SeedAdminUserAsync();

                // 3. Konaklama türlerini oluştur
                await SeedPropertyTypesAsync();

                // 4. Konumları oluştur
                await SeedLocationsAsync();

                // 5. Tedarikçileri oluştur
                await SeedSuppliersAsync();

                // 6. Konaklamaları oluştur
                await SeedPropertiesAsync();

                // 7. Birimleri oluştur
                await SeedUnitsAsync();

                // 8. Konaklama resimlerini oluştur
                await SeedPropertyImagesAsync();

                // 9. Rezervasyonları oluştur
                await SeedReservationsAsync();

                // 10. Ödemeleri oluştur
                await SeedPaymentsAsync();

                _logger.LogInformation("✅ Seed data başarıyla oluşturuldu!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Seed data oluşturulurken hata oluştu!");
                throw;
            }
        }

        private async Task SeedRolesAsync()
        {
            _logger.LogInformation("👥 Roller oluşturuluyor...");

            var roles = new[]
            {
                new ApplicationRole { Name = "Admin", Description = "Sistem yöneticisi" },
                new ApplicationRole { Name = "Supplier", Description = "Tedarikçi" },
                new ApplicationRole { Name = "Customer", Description = "Müşteri" }
            };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role.Name!))
                {
                    await _roleManager.CreateAsync(role);
                    _logger.LogInformation("✅ Rol oluşturuldu: {RoleName}", role.Name);
                }
            }
        }

        private async Task SeedAdminUserAsync()
        {
            _logger.LogInformation("👤 Admin kullanıcısı oluşturuluyor...");

            var adminUser = new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                FirstName = "Admin",
                LastName = "User",
                PhoneNumber = "5551234567",
                IsSupplier = false,
                CreatedDate = DateTime.UtcNow,
                LastLoginDate = DateTime.UtcNow,
                IsActive = true
            };

            var existingUser = await _userManager.FindByEmailAsync(adminUser.Email);
            if (existingUser == null)
            {
                var result = await _userManager.CreateAsync(adminUser, "Super123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                    _logger.LogInformation("✅ Admin kullanıcısı oluşturuldu: {Email}", adminUser.Email);
                }
                else
                {
                    _logger.LogError("❌ Admin kullanıcısı oluşturulamadı: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }

        private async Task SeedPropertyTypesAsync()
        {
            _logger.LogInformation("🏠 Konaklama türleri oluşturuluyor...");

            var propertyTypes = new[]
            {
                new PropertyType { Name = "Villa", Slug = "villa", Description = "Lüks villa konaklamaları", IconUrl = "/icons/villa.png", DisplayOrder = 1 },
                new PropertyType { Name = "Bungalov", Slug = "bungalov", Description = "Doğa ile iç içe bungalov konaklamaları", IconUrl = "/icons/bungalov.png", DisplayOrder = 2 },
                new PropertyType { Name = "Apart", Slug = "apart", Description = "Modern apart konaklamaları", IconUrl = "/icons/apart.png", DisplayOrder = 3 },
                new PropertyType { Name = "Ev", Slug = "ev", Description = "Aile evi konaklamaları", IconUrl = "/icons/ev.png", DisplayOrder = 4 },
                new PropertyType { Name = "Stüdyo", Slug = "stüdyo", Description = "Kompakt stüdyo konaklamaları", IconUrl = "/icons/stüdyo.png", DisplayOrder = 5 },
                new PropertyType { Name = "Çiftlik Evi", Slug = "ciftlik-evi", Description = "Kırsal çiftlik evi konaklamaları", IconUrl = "/icons/ciftlik.png", DisplayOrder = 6 },
                new PropertyType { Name = "Yat", Slug = "yat", Description = "Lüks yat konaklamaları", IconUrl = "/icons/yat.png", DisplayOrder = 7 },
                new PropertyType { Name = "Kamp", Slug = "kamp", Description = "Kamp ve çadır konaklamaları", IconUrl = "/icons/kamp.png", DisplayOrder = 8 }
            };

            foreach (var propertyType in propertyTypes)
            {
                if (!_context.PropertyTypes.Any(pt => pt.Name == propertyType.Name))
                {
                    _context.PropertyTypes.Add(propertyType);
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} konaklama türü oluşturuldu", propertyTypes.Length);
        }

        private async Task SeedLocationsAsync()
        {
            _logger.LogInformation("📍 Konumlar oluşturuluyor...");

            var cities = new[]
            {
                "Antalya", "Muğla", "İzmir", "Çanakkale", "Balıkesir", "Aydın", "Denizli", "Bursa",
                "İstanbul", "Trabzon", "Rize", "Artvin", "Giresun", "Ordu", "Samsun", "Sinop",
                "Kastamonu", "Zonguldak", "Bartın", "Karabük", "Bolu", "Düzce", "Sakarya", "Kocaeli"
            };

            var districts = new[]
            {
                "Merkez", "Kemer", "Kaş", "Kalkan", "Patara", "Çıralı", "Olimpos", "Side", "Alanya",
                "Manavgat", "Serik", "Aksu", "Döşemealtı", "Konyaaltı", "Muratpaşa", "Kepez"
            };

            var faker = new Faker<Location>("tr")
                .RuleFor(l => l.City, f => f.PickRandom(cities))
                .RuleFor(l => l.District, f => f.PickRandom(districts))
                .RuleFor(l => l.Region, f => f.Address.State())
                .RuleFor(l => l.Address, f => f.Address.FullAddress())
                .RuleFor(l => l.Latitude, f => f.Random.Decimal(36.0m, 42.0m))
                .RuleFor(l => l.Longitude, f => f.Random.Decimal(26.0m, 45.0m));

            var locations = faker.Generate(150); // 150 konum oluştur

            foreach (var location in locations)
            {
                if (!_context.Locations.Any(l => l.City == location.City && l.District == location.District))
                {
                    _context.Locations.Add(location);
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} konum oluşturuldu", locations.Count);
        }

        private async Task SeedSuppliersAsync()
        {
            _logger.LogInformation("🏢 Tedarikçiler oluşturuluyor...");

            var faker = new Faker<Supplier>("tr")
                .RuleFor(s => s.Name, f => f.Company.CompanyName())
                .RuleFor(s => s.TaxNumber, f => f.Random.Replace("##########"))
                .RuleFor(s => s.IBAN, f => f.Finance.Iban())
                .RuleFor(s => s.ContractStatus, f => f.PickRandom("Bekliyor", "Onaylı", "Aktif", "Pasif"))
                .RuleFor(s => s.Score, f => f.Random.Decimal(3.0m, 5.0m));

            var suppliers = faker.Generate(50); // 50 tedarikçi oluştur

            foreach (var supplier in suppliers)
            {
                if (!_context.Suppliers.Any(s => s.Name == supplier.Name))
                {
                    _context.Suppliers.Add(supplier);
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} tedarikçi oluşturuldu", suppliers.Count);
        }

        private async Task SeedPropertiesAsync()
        {
            _logger.LogInformation("🏡 Konaklamalar oluşturuluyor...");

            var propertyTypes = await _context.PropertyTypes.ToListAsync();
            var locations = await _context.Locations.ToListAsync();
            var suppliers = await _context.Suppliers.ToListAsync();

            // Debug için listelerin boyutunu kontrol et
            _logger.LogInformation("PropertyTypes count: {Count}", propertyTypes.Count);
            _logger.LogInformation("Locations count: {Count}", locations.Count);
            _logger.LogInformation("Suppliers count: {Count}", suppliers.Count);

            if (propertyTypes.Count == 0 || locations.Count == 0 || suppliers.Count == 0)
            {
                _logger.LogError("❌ PropertyTypes, Locations veya Suppliers listesi boş! Property oluşturulamıyor.");
                return;
            }

            var faker = new Faker<Property>("tr")
                .RuleFor(p => p.Title, f => f.Lorem.Sentence(3, 8))
                .RuleFor(p => p.Description, f => f.Lorem.Paragraphs(3, 5))
                .RuleFor(p => p.PropertyTypeId, f => f.PickRandom(propertyTypes).Id)
                .RuleFor(p => p.LocationId, f => f.PickRandom(locations).Id)
                .RuleFor(p => p.SupplierId, f => f.PickRandom(suppliers).Id)
                .RuleFor(p => p.Capacity, f => f.Random.Int(2, 20))
                .RuleFor(p => p.RoomCount, f => f.Random.Int(1, 8))
                .RuleFor(p => p.BathroomCount, f => f.Random.Int(1, 4))
                .RuleFor(p => p.SquareMeter, f => f.Random.Int(50, 500));

            var properties = faker.Generate(200); // 200 konaklama oluştur

            foreach (var property in properties)
            {
                _context.Properties.Add(property);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} konaklama oluşturuldu", properties.Count);
        }

        private async Task SeedUnitsAsync()
        {
            _logger.LogInformation("🏠 Birimler oluşturuluyor...");

            var properties = await _context.Properties.ToListAsync();

            var faker = new Faker<Unit>("tr")
                .RuleFor(u => u.Name, f => f.Lorem.Word())
                .RuleFor(u => u.Description, f => f.Lorem.Sentence())
                .RuleFor(u => u.Price, f => f.Random.Decimal(500, 5000))
                .RuleFor(u => u.Currency, "TRY")
                .RuleFor(u => u.PropertyId, f => f.PickRandom(properties).Id);

            var units = faker.Generate(500); // 500 birim oluştur

            foreach (var unit in units)
            {
                _context.Units.Add(unit);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} birim oluşturuldu", units.Count);
        }

        private async Task SeedPropertyImagesAsync()
        {
            _logger.LogInformation("📸 Konaklama resimleri oluşturuluyor...");

            var properties = await _context.Properties.ToListAsync();
            var imageCategories = new[] { "LivingRoom", "Kitchen", "Bedroom", "Bathroom", "Exterior", "Pool", "Garden", "View" };

            var faker = new Faker<PropertyImage>("tr")
                .RuleFor(pi => pi.ImageUrl, f => f.Image.PicsumUrl(800, 600))
                .RuleFor(pi => pi.AltText, f => f.Lorem.Sentence(2, 4))
                .RuleFor(pi => pi.Order, f => f.Random.Int(1, 10))
                .RuleFor(pi => pi.PropertyId, f => f.PickRandom(properties).Id);

            var images = faker.Generate(1000); // 1000 resim oluştur

            foreach (var image in images)
            {
                _context.PropertyImages.Add(image);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} konaklama resmi oluşturuldu", images.Count);
        }

        private async Task SeedReservationsAsync()
        {
            _logger.LogInformation("📅 Rezervasyonlar oluşturuluyor...");

            var properties = await _context.Properties.ToListAsync();
            var users = await _context.Users.ToListAsync();
            var statuses = new[] { ReservationStatus.Pending, ReservationStatus.Confirmed, ReservationStatus.CancelledByGuest, ReservationStatus.Completed, ReservationStatus.NoShow };

            var faker = new Faker<Reservation>("tr")
                .RuleFor(r => r.PropertyId, f => f.PickRandom(properties).Id)
                .RuleFor(r => r.UserId, f => f.PickRandom(users).Id)
                .RuleFor(r => r.CheckInDate, f => f.Date.Between(DateTime.Now.AddDays(-30), DateTime.Now.AddDays(365)))
                .RuleFor(r => r.CheckOutDate, (f, r) => r.CheckInDate.AddDays(f.Random.Int(1, 14)))
                .RuleFor(r => r.TotalPrice, f => f.Random.Decimal(1000, 10000))
                .RuleFor(r => r.Currency, "TRY")
                .RuleFor(r => r.Status, f => f.PickRandom(statuses))
                .RuleFor(r => r.GuestName, f => f.Name.FullName())
                .RuleFor(r => r.GuestEmail, f => f.Internet.Email())
                .RuleFor(r => r.GuestPhone, f => f.Phone.PhoneNumber())
                .RuleFor(r => r.GuestCount, f => f.Random.Int(1, 8));

            var reservations = faker.Generate(300); // 300 rezervasyon oluştur

            foreach (var reservation in reservations)
            {
                _context.Reservations.Add(reservation);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} rezervasyon oluşturuldu", reservations.Count);
        }

        private async Task SeedPaymentsAsync()
        {
            _logger.LogInformation("💳 Ödemeler oluşturuluyor...");

            var reservations = await _context.Reservations.ToListAsync();
            var paymentMethods = new[] { PaymentMethod.CreditCard, PaymentMethod.BankTransfer, PaymentMethod.Cash, PaymentMethod.PayPal, PaymentMethod.Stripe };
            var statuses = new[] { PaymentStatus.Pending, PaymentStatus.Completed, PaymentStatus.Failed, PaymentStatus.Refunded, PaymentStatus.Cancelled };

            var faker = new Faker<Payment>("tr")
                .RuleFor(p => p.ReservationId, f => f.PickRandom(reservations).Id)
                .RuleFor(p => p.Amount, f => f.Random.Decimal(500, 8000))
                .RuleFor(p => p.Currency, "TRY")
                .RuleFor(p => p.Method, f => f.PickRandom(paymentMethods))
                .RuleFor(p => p.Status, f => f.PickRandom(statuses))
                .RuleFor(p => p.TransactionRef, f => f.Random.AlphaNumeric(20));

            var payments = faker.Generate(250); // 250 ödeme oluştur

            foreach (var payment in payments)
            {
                _context.Payments.Add(payment);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} ödeme oluşturuldu", payments.Count);
        }
    }
}