using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Trimango.Data.Mssql.Entities;
using Trimango.Data.Mssql.Enums;
using Trimango.Mssql;
using Bogus;
using Maggsoft.Core.Base;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Trimango.Api.Services
{
    /// <summary>
    /// Provinces.json dosyasından okunan il verisi için model sınıfı
    /// </summary>
    public class ProvinceData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("districts")]
        public List<DistrictData> Districts { get; set; } = new List<DistrictData>();
    }

    /// <summary>
    /// Provinces.json dosyasından okunan ilçe verisi için model sınıfı
    /// </summary>
    public class DistrictData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
    /// <summary>
    /// Kapsamlı seed data servisi - Tüm tablolara ilişkili veriler oluşturur
    /// </summary>
    public class SeedDataService
    {
        private readonly TrimangoDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<SeedDataService> _logger;
        private Guid? _adminUserId;

        // Faker instances
        private readonly Faker _faker;
        private readonly Faker<ApplicationUser> _userFaker;
        private readonly Faker<PropertyType> _propertyTypeFaker;
        private readonly Faker<Location> _locationFaker;
        private readonly Faker<Supplier> _supplierFaker;
        private readonly Faker<Property> _propertyFaker;
        private readonly Faker<Unit> _unitFaker;
        private readonly Faker<PropertyImage> _propertyImageFaker;
        private readonly Faker<Feature> _featureFaker;
        private readonly Faker<Language> _languageFaker;
        private readonly Faker<City> _cityFaker;
        private readonly Faker<District> _districtFaker;
        private readonly Faker<BlogCategory> _blogCategoryFaker;
        private readonly Faker<BlogPost> _blogPostFaker;
        private readonly Faker<Coupon> _couponFaker;
        private readonly Faker<Notification> _notificationFaker;

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

            // Faker instances oluştur
            _faker = new Faker("tr");
            
            _userFaker = new Faker<ApplicationUser>("tr")
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.ProfilePictureUrl, f => f.Image.PicsumUrl())
                .RuleFor(u => u.IsSupplier, f => f.Random.Bool())
                .RuleFor(u => u.EmailConfirmed, f => true)
                .RuleFor(u => u.CreatedDate, f => f.Date.Past())
                .RuleFor(u => u.LastLoginDate, f => f.Date.Recent())
                .RuleFor(u => u.IsActive, f => true);

            _propertyTypeFaker = new Faker<PropertyType>("tr")
                .RuleFor(pt => pt.Name, f => f.PickRandom("Villa", "Apartman", "Ev", "Stüdyo", "Loft", "Çiftlik Evi", "Yazlık", "Köşk"))
                .RuleFor(pt => pt.Slug, (f, pt) => pt.Name.ToLowerInvariant().Replace(" ", "-"))
                .RuleFor(pt => pt.Description, f => f.Lorem.Sentence())
                .RuleFor(pt => pt.IconUrl, f => f.Image.PicsumUrl())
                .RuleFor(pt => pt.IsActive, f => true);

            _locationFaker = new Faker<Location>("tr")
                .RuleFor(l => l.Region, f => f.PickRandom("Marmara", "Ege", "Akdeniz", "İç Anadolu", "Karadeniz"))
                .RuleFor(l => l.Address, f => f.Address.FullAddress())
                .RuleFor(l => l.Latitude, f => f.Random.Decimal(35m, 42m))
                .RuleFor(l => l.Longitude, f => f.Random.Decimal(25m, 45m))
                .RuleFor(l => l.DisplayOrder, f => f.Random.Int(0, 100))
                .RuleFor(l => l.IsActive, f => true);

            _supplierFaker = new Faker<Supplier>("tr")
                .RuleFor(s => s.Name, f => f.Company.CompanyName())
                .RuleFor(s => s.TaxNumber, f => f.Random.Replace("##########"))
                .RuleFor(s => s.IBAN, f => f.Finance.Iban())
                .RuleFor(s => s.ContractStatus, f => f.PickRandom("Active", "Pending", "Suspended"))
                .RuleFor(s => s.Score, f => f.Random.Decimal(1, 5))
                .RuleFor(s => s.IsActive, f => true);

            _propertyFaker = new Faker<Property>("tr")
                .RuleFor(p => p.Title, f => f.Lorem.Sentence(3))
                .RuleFor(p => p.Description, f => f.Lorem.Paragraphs(2))
                .RuleFor(p => p.IsActive, f => true);

            _unitFaker = new Faker<Unit>("tr")
                .RuleFor(u => u.Name, f => f.PickRandom("Standart Oda", "Deluxe Oda", "Suit", "Villa", "Apartman Dairesi"))
                .RuleFor(u => u.Description, f => f.Lorem.Sentence())
                .RuleFor(u => u.Capacity, f => f.Random.Int(1, 8))
                .RuleFor(u => u.BedConfig, f => f.PickRandom("1 Yatak", "2 Yatak", "3 Yatak", "1+1", "2+1", "3+1"))
                .RuleFor(u => u.PrivatePool, f => f.Random.Bool())
                .RuleFor(u => u.Price, f => f.Random.Decimal(100, 2000))
                .RuleFor(u => u.Currency, f => "TRY")
                .RuleFor(u => u.IsActive, f => true);

            _propertyImageFaker = new Faker<PropertyImage>("tr")
                .RuleFor(pi => pi.ImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(pi => pi.Category, f => f.PickRandom("Exterior", "Interior", "Kitchen", "Bathroom", "Bedroom", "Living Room"))
                .RuleFor(pi => pi.IsActive, f => true);

            _featureFaker = new Faker<Feature>("tr")
                .RuleFor(f => f.Name, f => f.PickRandom("WiFi", "Havuz", "Spa", "Fitness", "Parking", "Balcony", "Garden", "Sea View"))
                .RuleFor(f => f.Category, f => f.PickRandom<FeatureCategory>())
                .RuleFor(f => f.IconUrl, f => f.Image.PicsumUrl())
                .RuleFor(f => f.IsActive, f => true);

            _languageFaker = new Faker<Language>("tr")
                .RuleFor(l => l.Name, f => f.PickRandom("Türkçe", "English", "Deutsch", "Français", "Español"))
                .RuleFor(l => l.LanguageCulture, f => f.PickRandom("tr-TR", "en-US", "de-DE", "fr-FR", "es-ES"))
                .RuleFor(l => l.UniqueSeoCode, f => f.PickRandom("tr", "en", "de", "fr", "es"))
                .RuleFor(l => l.FlagImageFileName, f => f.Image.PicsumUrl())
                .RuleFor(l => l.Rtl, f => false)
                .RuleFor(l => l.State, f => f.PickRandom<LanguageState>())
                .RuleFor(l => l.DisplayOrder, f => f.Random.Int(0, 10))
                .RuleFor(l => l.IsActive, f => true);

            _cityFaker = new Faker<City>("tr")
                .RuleFor(c => c.Name, f => f.PickRandom("İstanbul", "Ankara", "İzmir", "Antalya", "Bodrum", "Çeşme", "Marmaris", "Kuşadası"))
                .RuleFor(c => c.IsActive, f => true);

            _districtFaker = new Faker<District>("tr")
                .RuleFor(d => d.Name, f => f.PickRandom("Beşiktaş", "Kadıköy", "Şişli", "Beyoğlu", "Üsküdar", "Maltepe", "Kartal", "Pendik"))
                .RuleFor(d => d.IsActive, f => true);

            _blogCategoryFaker = new Faker<BlogCategory>("tr")
                .RuleFor(bc => bc.Name, f => f.PickRandom("Seyahat", "Konaklama", "Yemek", "Aktiviteler", "Kültür", "Doğa"))
                .RuleFor(bc => bc.Slug, (f, bc) => bc.Name.ToLowerInvariant().Replace(" ", "-"))
                .RuleFor(bc => bc.Description, f => f.Lorem.Sentence())
                .RuleFor(bc => bc.IsActive, f => true);

            _blogPostFaker = new Faker<BlogPost>("tr")
                .RuleFor(bp => bp.Title, f => f.Lorem.Sentence(4))
                .RuleFor(bp => bp.Slug, (f, bp) => bp.Title.ToLowerInvariant().Replace(" ", "-"))
                .RuleFor(bp => bp.Content, f => f.Lorem.Paragraphs(5))
                .RuleFor(bp => bp.FeaturedImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(bp => bp.IsActive, f => true);

            _couponFaker = new Faker<Coupon>("tr")
                .RuleFor(c => c.Code, f => f.Random.AlphaNumeric(8).ToUpper())
                .RuleFor(c => c.Name, f => f.Lorem.Sentence(2))
                .RuleFor(c => c.Type, f => f.PickRandom<CouponType>())
                .RuleFor(c => c.Value, f => f.Random.Decimal(10, 500))
                .RuleFor(c => c.MinOrderAmount, f => f.Random.Decimal(100, 1000))
                .RuleFor(c => c.MaxDiscountAmount, f => f.Random.Decimal(50, 200))
                .RuleFor(c => c.UsageLimit, f => f.Random.Int(10, 1000))
                .RuleFor(c => c.UsedCount, f => 0)
                .RuleFor(c => c.StartDate, f => f.Date.Between(DateTime.Now.AddDays(-30), DateTime.Now.AddDays(30)))
                .RuleFor(c => c.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(90)))
                .RuleFor(c => c.IsActive, f => true);

            _notificationFaker = new Faker<Notification>("tr")
                .RuleFor(n => n.Title, f => f.Lorem.Sentence(3))
                .RuleFor(n => n.Message, f => f.Lorem.Sentence())
                .RuleFor(n => n.Type, f => f.PickRandom<NotificationType>())
                .RuleFor(n => n.IsRead, f => f.Random.Bool())
                .RuleFor(n => n.CreatedDate, f => f.Date.Past())
                .RuleFor(n => n.IsActive, f => true);
        }

        /// <summary>
        /// Tüm seed data'yı oluşturur
        /// </summary>
        public async Task SeedAllDataAsync()
        {
            try
            {
                _logger.LogInformation("🌱 Kapsamlı seed data oluşturma başlatılıyor...");

                // 1. Rolleri oluştur
                await SeedRolesAsync();

                // 2. Admin kullanıcısını oluştur
                await SeedAdminUserAsync();

                // 2.1. Supplier kullanıcılarını oluştur
                await SeedSupplierUsersAsync();

                // 2.2. Customer kullanıcılarını oluştur
                await SeedCustomerUsersAsync();

                // 3. Dilleri oluştur
                await SeedLanguagesAsync();

                // 4. Şehirleri oluştur
                await SeedCitiesAsync();

                // 5. İlçeleri oluştur
                await SeedDistrictsAsync();

                // 6. Konaklama türlerini oluştur
                await SeedPropertyTypesAsync();

                // 7. Konumları oluştur
                await SeedLocationsAsync();

                // 8. Tedarikçileri oluştur
                await SeedSuppliersAsync();

                // 9. Özellikleri oluştur
                await SeedFeaturesAsync();

                // 10. Konaklamaları oluştur
                await SeedPropertiesAsync();

                // 11. Birimleri oluştur
                await SeedUnitsAsync();

                // 12. Konaklama resimlerini oluştur
                await SeedPropertyImagesAsync();

                // 13. Özellik eşleştirmelerini oluştur
                await SeedPropertyFeatureMappingsAsync();

                // 14. Mesafe bilgilerini oluştur
                await SeedDistanceInfosAsync();

                // 15. Mevsimsel fiyatlandırmaları oluştur
                await SeedSeasonalPricingsAsync();

                // 16. Kalış kurallarını oluştur
                await SeedStayRulesAsync();

                // 17. Ek ücretleri oluştur
                await SeedExtraFeesAsync();

                // 18. Politikaları oluştur
                await SeedPoliciesAsync();

                // 19. Rezervasyonları oluştur
                await SeedReservationsAsync();

                // 20. Rezervasyon fiyat detaylarını oluştur
                await SeedReservationPriceBreakdownsAsync();

                // 21. Ödemeleri oluştur
                await SeedPaymentsAsync();

                // 22. Sayfaları oluştur
                await SeedPagesAsync();

                // 23. SEO içeriklerini oluştur
                await SeedSeoContentsAsync();

                // 24. Blog kategorilerini oluştur
                await SeedBlogCategoriesAsync();

                // 25. Blog yazılarını oluştur
                await SeedBlogPostsAsync();

                // 26. Blog yorumlarını oluştur
                await SeedBlogCommentsAsync();

                // 27. Blog etiketlerini oluştur
                await SeedBlogTagsAsync();

                // 28. Sayfa yorumlarını oluştur
                await SeedPageCommentsAsync();

                // 29. Değerlendirmeleri oluştur
                await SeedReviewsAsync();

                // 30. Soruları oluştur
                await SeedQuestionsAsync();

                // 31. Favorileri oluştur
                await SeedFavoritesAsync();

                // 32. Mesajları oluştur
                await SeedMessagesAsync();

                // 33. Anlaşmazlıkları oluştur
                await SeedDisputesAsync();

                // 34. Kuponları oluştur
                await SeedCouponsAsync();

                // 35. Kupon kullanımlarını oluştur
                await SeedCouponUsagesAsync();

                // 36. Bildirimleri oluştur
                await SeedNotificationsAsync();

                // 37. Sistem loglarını oluştur
                await SeedSystemLogsAsync();

                // 38. Lokalizasyon kaynaklarını oluştur
                await SeedLocaleStringResourcesAsync();

                // 39. Lokalize özellikleri oluştur
                await SeedLocalizedPropertiesAsync();

                _logger.LogInformation("✅ Kapsamlı seed data başarıyla oluşturuldu!");
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
                PhoneNumber = "5373761004",
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
                    _adminUserId = adminUser.Id;
                    _logger.LogInformation("✅ Admin kullanıcısı oluşturuldu: {Email}", adminUser.Email);
                }
                else
                {
                    _logger.LogError("❌ Admin kullanıcısı oluşturulamadı: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }

        private async Task SeedSupplierUsersAsync()
        {
            _logger.LogInformation("🏢 Supplier kullanıcıları oluşturuluyor...");

            // Mevcut supplier kullanıcıları kontrol et
            var existingSuppliers = await _context.Users.Where(u => u.IsSupplier == true).ToListAsync();
            if (existingSuppliers.Count >= 5)
            {
                _logger.LogInformation("ℹ️ Supplier kullanıcıları zaten mevcut, atlanıyor...");
                return;
            }

            var supplierUsers = new List<ApplicationUser>();
            for (int i = 1; i <= 5; i++)
            {
                var supplier = new ApplicationUser
                {
                    UserName = $"supplier{i}@gmail.com",
                    Email = $"supplier{i}@gmail.com",
                    FirstName = _faker.Name.FirstName(),
                    LastName = _faker.Name.LastName(),
                    PhoneNumber = _faker.Phone.PhoneNumber(),
                    IsSupplier = true,
                    CreatedDate = DateTime.UtcNow,
                    LastLoginDate = DateTime.UtcNow,
                    IsActive = true
                };
                supplierUsers.Add(supplier);
            }

            foreach (var supplier in supplierUsers)
            {
                var existingUser = await _userManager.FindByEmailAsync(supplier.Email);
                if (existingUser == null)
                {
                    var result = await _userManager.CreateAsync(supplier, "Supplier123!");
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(supplier, "Supplier");
                        _logger.LogInformation("✅ Supplier kullanıcısı oluşturuldu: {Email}", supplier.Email);
                    }
                    else
                    {
                        _logger.LogError("❌ Supplier kullanıcısı oluşturulamadı: {Email} - {Errors}", 
                            supplier.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
        }

        private async Task SeedCustomerUsersAsync()
        {
            _logger.LogInformation("👥 Customer kullanıcıları oluşturuluyor...");

            // Mevcut customer kullanıcıları kontrol et
            var existingCustomers = await _context.Users.Where(u => u.IsSupplier == false && u.Email != "admin@gmail.com").ToListAsync();
            if (existingCustomers.Count >= 5)
            {
                _logger.LogInformation("ℹ️ Customer kullanıcıları zaten mevcut, atlanıyor...");
                return;
            }

            var customerUsers = new List<ApplicationUser>();
            for (int i = 1; i <= 5; i++)
            {
                var customer = new ApplicationUser
                {
                    UserName = $"customer{i}@gmail.com",
                    Email = $"customer{i}@gmail.com",
                    FirstName = _faker.Name.FirstName(),
                    LastName = _faker.Name.LastName(),
                    PhoneNumber = _faker.Phone.PhoneNumber(),
                    IsSupplier = false,
                    CreatedDate = DateTime.UtcNow,
                    LastLoginDate = DateTime.UtcNow,
                    IsActive = true
                };
                customerUsers.Add(customer);
            }

            foreach (var customer in customerUsers)
            {
                var existingUser = await _userManager.FindByEmailAsync(customer.Email);
                if (existingUser == null)
                {
                    var result = await _userManager.CreateAsync(customer, "Customer123!");
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(customer, "Customer");
                        _logger.LogInformation("✅ Customer kullanıcısı oluşturuldu: {Email}", customer.Email);
                    }
                    else
                    {
                        _logger.LogError("❌ Customer kullanıcısı oluşturulamadı: {Email} - {Errors}", 
                            customer.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
        }

        private async Task SeedLanguagesAsync()
        {
            _logger.LogInformation("🌍 Diller oluşturuluyor...");

            // Mevcut dilleri kontrol et
            var existingLanguages = await _context.Languages.ToListAsync();
            var existingSeoCodes = existingLanguages.Select(l => l.UniqueSeoCode).ToHashSet();

            var languages = new List<Language>
            {
                new Language
                {
                    Name = "Türkçe",
                    LanguageCulture = "tr-TR",
                    UniqueSeoCode = "tr",
                    FlagImageFileName = "turkey.png",
                    Rtl = false,
                    State = LanguageState.Active,
                    DisplayOrder = 1,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new Language
                {
                    Name = "English",
                    LanguageCulture = "en-US",
                    UniqueSeoCode = "en",
                    FlagImageFileName = "united-states.png",
                    Rtl = false,
                    State = LanguageState.Active,
                    DisplayOrder = 2,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new Language
                {
                    Name = "Deutsch",
                    LanguageCulture = "de-DE",
                    UniqueSeoCode = "de",
                    FlagImageFileName = "germany.png",
                    Rtl = false,
                    State = LanguageState.Active,
                    DisplayOrder = 3,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new Language
                {
                    Name = "Français",
                    LanguageCulture = "fr-FR",
                    UniqueSeoCode = "fr",
                    FlagImageFileName = "france.png",
                    Rtl = false,
                    State = LanguageState.Active,
                    DisplayOrder = 4,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new Language
                {
                    Name = "Español",
                    LanguageCulture = "es-ES",
                    UniqueSeoCode = "es",
                    FlagImageFileName = "spain.png",
                    Rtl = false,
                    State = LanguageState.Active,
                    DisplayOrder = 5,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                }
            };

            // Sadece mevcut olmayan dilleri ekle
            var newLanguages = languages.Where(l => !existingSeoCodes.Contains(l.UniqueSeoCode)).ToList();
            
            if (newLanguages.Any())
            {
                _context.Languages.AddRange(newLanguages);
                await _context.SaveChangesAsync();
                _logger.LogInformation("✅ {Count} yeni dil oluşturuldu", newLanguages.Count);
            }
            else
            {
                _logger.LogInformation("ℹ️ Tüm diller zaten mevcut, yeni dil eklenmedi");
            }
        }

        private async Task SeedCitiesAsync()
        {
            _logger.LogInformation("🏙️ Şehirler oluşturuluyor...");

            // Provinces.json dosyasından şehirleri oku
            var provincesJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "provinces.json");
            var provincesJson = await File.ReadAllTextAsync(provincesJsonPath);
            var provinces = System.Text.Json.JsonSerializer.Deserialize<List<ProvinceData>>(provincesJson);

            if (provinces == null || !provinces.Any())
            {
                _logger.LogWarning("⚠️ Provinces.json dosyası okunamadı, varsayılan şehirler oluşturuluyor...");
                var cities = _cityFaker.Generate(10);
                _context.Cities.AddRange(cities);
                await _context.SaveChangesAsync();
                _logger.LogInformation("✅ {Count} varsayılan şehir oluşturuldu", cities.Count);
                return;
            }

            var cityEntities = provinces
                .Where(p=>!string.IsNullOrEmpty(p.Name))
                .Select(p => new City
                {
                    Name = p.Name,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                }).ToList();

            _context.Cities.AddRange(cityEntities);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} şehir provinces.json'dan oluşturuldu", cityEntities.Count);
        }

        private async Task SeedDistrictsAsync()
        {
            _logger.LogInformation("🏘️ İlçeler oluşturuluyor...");

            // Provinces.json dosyasından ilçeleri oku
            var provincesJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "provinces.json");
            var provincesJson = await File.ReadAllTextAsync(provincesJsonPath);
            var provinces = System.Text.Json.JsonSerializer.Deserialize<List<ProvinceData>>(provincesJson);

            if (provinces == null || !provinces.Any())
            {
                _logger.LogWarning("⚠️ Provinces.json dosyası okunamadı, varsayılan ilçeler oluşturuluyor...");
                var cities = await _context.Cities.ToListAsync();
                var districts = new List<District>();

                foreach (var city in cities)
                {
                    var cityDistricts = _districtFaker.Generate(3);
                    foreach (var district in cityDistricts)
                    {
                        district.CityId = city.Id;
                    }
                    districts.AddRange(cityDistricts);
                }

                _context.Districts.AddRange(districts);
                await _context.SaveChangesAsync();
                _logger.LogInformation("✅ {Count} varsayılan ilçe oluşturuldu", districts.Count);
                return;
            }

            var cityEntities = await _context.Cities.ToListAsync();
            var districtEntities = new List<District>();

            foreach (var province in provinces)
            {
                var city = cityEntities.FirstOrDefault(c => c.Name == province.Name);
                if (city == null) continue;

                foreach (var districtData in province.Districts)
                {
                    var district = new District
                    {
                        Name = districtData.Name,
                        CityId = city.Id,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow
                    };
                    districtEntities.Add(district);
                }
            }

            _context.Districts.AddRange(districtEntities);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} ilçe provinces.json'dan oluşturuldu", districtEntities.Count);
        }

        private async Task SeedPropertyTypesAsync()
        {
            _logger.LogInformation("🏠 Konaklama türleri oluşturuluyor...");

            var propertyTypes = _propertyTypeFaker.Generate(8);
            _context.PropertyTypes.AddRange(propertyTypes);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} konaklama türü oluşturuldu", propertyTypes.Count);
        }

        private async Task SeedLocationsAsync()
        {
            _logger.LogInformation("📍 Konumlar oluşturuluyor...");

            var cities = await _context.Cities.ToListAsync();
            var districts = await _context.Districts.ToListAsync();
            
            if (!cities.Any() || !districts.Any())
            {
                _logger.LogWarning("⚠️ Şehir veya ilçe bulunamadı, konum oluşturulamıyor");
                return;
            }

            var locations = new List<Location>();
            for (int i = 0; i < 50; i++)
            {
                var location = _locationFaker.Generate();
                location.CityId = _faker.PickRandom(cities).Id;
                location.DistrictId = _faker.PickRandom(districts).Id;
                locations.Add(location);
            }

            _context.Locations.AddRange(locations);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} konum oluşturuldu", locations.Count);
        }

        private async Task SeedSuppliersAsync()
        {
            _logger.LogInformation("🏢 Tedarikçiler oluşturuluyor...");

            var suppliers = _supplierFaker.Generate(20);
            _context.Suppliers.AddRange(suppliers);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} tedarikçi oluşturuldu", suppliers.Count);
        }

        private async Task SeedFeaturesAsync()
        {
            _logger.LogInformation("⭐ Özellikler oluşturuluyor...");

            var features = _featureFaker.Generate(30);
            _context.Features.AddRange(features);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} özellik oluşturuldu", features.Count);
        }

        private async Task SeedPropertiesAsync()
        {
            _logger.LogInformation("🏡 Konaklamalar oluşturuluyor...");

            var propertyTypes = await _context.PropertyTypes.ToListAsync();
            var locations = await _context.Locations.ToListAsync();
            var suppliers = await _context.Suppliers.ToListAsync();
            var cities = await _context.Cities.ToListAsync();
            var districts = await _context.Districts.ToListAsync();

            var properties = new List<Property>();
            for (int i = 0; i < 100; i++)
            {
                var property = _propertyFaker.Generate();
                property.PropertyTypeId = _faker.PickRandom(propertyTypes).Id;
                property.LocationId = _faker.PickRandom(locations).Id;
                property.SupplierId = _faker.PickRandom(suppliers).Id;
                property.CityId = _faker.PickRandom(cities).Id;
                property.DistrictId = _faker.PickRandom(districts).Id;
                properties.Add(property);
            }

            _context.Properties.AddRange(properties);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} konaklama oluşturuldu", properties.Count);
        }

        private async Task SeedUnitsAsync()
        {
            _logger.LogInformation("🏠 Birimler oluşturuluyor...");

            var properties = await _context.Properties.ToListAsync();
            var units = new List<Unit>();

            foreach (var property in properties)
            {
                var propertyUnits = _unitFaker.Generate(_faker.Random.Int(1, 5));
                foreach (var unit in propertyUnits)
                {
                    unit.PropertyId = property.Id;
                }
                units.AddRange(propertyUnits);
            }

            _context.Units.AddRange(units);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} birim oluşturuldu", units.Count);
        }

        private async Task SeedPropertyImagesAsync()
        {
            _logger.LogInformation("📸 Konaklama resimleri oluşturuluyor...");

            var properties = await _context.Properties.ToListAsync();
            var images = new List<PropertyImage>();

            foreach (var property in properties)
            {
                var propertyImages = _propertyImageFaker.Generate(_faker.Random.Int(3, 10));
                foreach (var image in propertyImages)
                {
                    image.PropertyId = property.Id;
                }
                images.AddRange(propertyImages);
            }

            _context.PropertyImages.AddRange(images);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} konaklama resmi oluşturuldu", images.Count);
        }

        private async Task SeedPropertyFeatureMappingsAsync()
        {
            _logger.LogInformation("🔗 Özellik eşleştirmeleri oluşturuluyor...");

            var properties = await _context.Properties.ToListAsync();
            var features = await _context.Features.ToListAsync();
            var mappings = new List<PropertyFeatureMapping>();

            foreach (var property in properties)
            {
                var selectedFeatures = _faker.PickRandom(features, _faker.Random.Int(3, 8));
                foreach (var feature in selectedFeatures)
                {
                    mappings.Add(new PropertyFeatureMapping
                    {
                        Id = Guid.NewGuid(),
                        PropertyId = property.Id,
                        FeatureId = feature.Id,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow
                    });
                }
            }

            _context.PropertyFeatureMappings.AddRange(mappings);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} özellik eşleştirmesi oluşturuldu", mappings.Count);
        }

        private async Task SeedDistanceInfosAsync()
        {
            _logger.LogInformation("📏 Mesafe bilgileri oluşturuluyor...");

            var properties = await _context.Properties.ToListAsync();
            var distanceInfos = new List<DistanceInfo>();

            foreach (var property in properties)
            {
                var propertyDistances = new Faker<DistanceInfo>("tr")
                    .RuleFor(di => di.Id, f => Guid.NewGuid())
                    .RuleFor(di => di.PlaceName, f => f.PickRandom("Havaalanı", "Merkez", "Plaj", "Alışveriş Merkezi", "Hastane", "Okul"))
                    .RuleFor(di => di.PlaceType, f => f.PickRandom<PlaceType>())
                    .RuleFor(di => di.DistanceKm, f => f.Random.Decimal(0.5m, 50m))
                    .RuleFor(di => di.PropertyId, property.Id)
                    .RuleFor(di => di.CreatedDate, f => DateTime.UtcNow)
                    .RuleFor(di => di.IsActive, f => true)
                    .Generate(_faker.Random.Int(2, 6));

                distanceInfos.AddRange(propertyDistances);
            }

            _context.DistanceInfos.AddRange(distanceInfos);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} mesafe bilgisi oluşturuldu", distanceInfos.Count);
        }

        private async Task SeedSeasonalPricingsAsync()
        {
            _logger.LogInformation("💰 Mevsimsel fiyatlandırmalar oluşturuluyor...");

            var properties = await _context.Properties.ToListAsync();
            var units = await _context.Units.ToListAsync();
            var seasonalPricings = new List<SeasonalPricing>();

            foreach (var property in properties)
            {
                var propertyPricings = new Faker<SeasonalPricing>("tr")
                    .RuleFor(sp => sp.Name, f => f.PickRandom("Yaz Sezonu", "Kış Sezonu", "Bahar Sezonu", "Yılbaşı", "Bayram"))
                    .RuleFor(sp => sp.Scope, f => f.PickRandom<ScopeType>())
                    .RuleFor(sp => sp.ScopeId, f => f.PickRandom(units.Where(u => u.PropertyId == property.Id)).Id)
                    .RuleFor(sp => sp.PricePerNight, f => f.Random.Decimal(100, 2000))
                    .RuleFor(sp => sp.Currency, f => "TRY")
                    .RuleFor(sp => sp.StartDate, f => f.Date.Between(DateTime.Now.AddDays(-30), DateTime.Now.AddDays(30)))
                    .RuleFor(sp => sp.EndDate, f => f.Date.Between(DateTime.Now.AddDays(30), DateTime.Now.AddDays(90)))
                    .RuleFor(sp => sp.IsActive, f => true)
                    .Generate(_faker.Random.Int(2, 5));

                seasonalPricings.AddRange(propertyPricings);
            }

            _context.SeasonalPricings.AddRange(seasonalPricings);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} mevsimsel fiyatlandırma oluşturuldu", seasonalPricings.Count);
        }

        private async Task SeedStayRulesAsync()
        {
            _logger.LogInformation("📋 Kalış kuralları oluşturuluyor...");

            var properties = await _context.Properties.ToListAsync();
            var units = await _context.Units.ToListAsync();
            var stayRules = new List<StayRule>();

            foreach (var property in properties)
            {
                var propertyRules = new Faker<StayRule>("tr")
                    .RuleFor(sr => sr.Scope, f => f.PickRandom<ScopeType>())
                    .RuleFor(sr => sr.ScopeId, f => f.PickRandom(units.Where(u => u.PropertyId == property.Id)).Id)
                    .RuleFor(sr => sr.CheckInDays, f => f.PickRandom("Pazartesi-Cumartesi", "Her gün", "Sadece Cuma-Pazar"))
                .RuleFor(sr => sr.MinNights, f => f.Random.Int(1, 7))
                .RuleFor(sr => sr.MaxNights, f => f.Random.Int(7, 30))
                    .RuleFor(sr => sr.IsActive, f => true)
                    .Generate(_faker.Random.Int(1, 3));

                stayRules.AddRange(propertyRules);
            }

            _context.StayRules.AddRange(stayRules);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} kalış kuralı oluşturuldu", stayRules.Count);
        }

        private async Task SeedExtraFeesAsync()
        {
            _logger.LogInformation("💸 Ek ücretler oluşturuluyor...");

            var properties = await _context.Properties.ToListAsync();
            var units = await _context.Units.ToListAsync();
            var extraFees = new List<ExtraFee>();

            foreach (var property in properties)
            {
                var propertyFees = new Faker<ExtraFee>("tr")
                    .RuleFor(ef => ef.Scope, f => f.PickRandom<ScopeType>())
                    .RuleFor(ef => ef.ScopeId, f => f.PickRandom(units.Where(u => u.PropertyId == property.Id)).Id)
                    .RuleFor(ef => ef.Name, f => f.PickRandom("Temizlik Ücreti", "Güvenlik Depozitosu", "Pet Ücreti", "Ek Yatak", "Havaalanı Transferi"))
                    .RuleFor(ef => ef.Type, f => f.PickRandom<ExtraFeeType>())
                    .RuleFor(ef => ef.Amount, f => f.Random.Decimal(10, 200))
                    .RuleFor(ef => ef.Description, f => f.Lorem.Sentence())
                    .RuleFor(ef => ef.IsActive, f => true)
                    .Generate(_faker.Random.Int(1, 4));

                extraFees.AddRange(propertyFees);
            }

            _context.ExtraFees.AddRange(extraFees);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} ek ücret oluşturuldu", extraFees.Count);
        }

        private async Task SeedPoliciesAsync()
        {
            _logger.LogInformation("📜 Politikalar oluşturuluyor...");

            var policies = new Faker<Policy>("tr")
                .RuleFor(p => p.PolicyType, f => f.PickRandom<PolicyType>())
                .RuleFor(p => p.Name, f => f.PickRandom("İptal Politikası", "Giriş-Çıkış Saatleri", "Evcil Hayvan Politikası", "Sigara Politikası", "Gürültü Politikası"))
                .RuleFor(p => p.Terms, f => f.Lorem.Paragraphs(2))
                .RuleFor(p => p.IsActive, f => true)
                .Generate(10);

            _context.Policies.AddRange(policies);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} politika oluşturuldu", policies.Count);
        }

        private async Task SeedReservationsAsync()
        {
            _logger.LogInformation("📅 Rezervasyonlar oluşturuluyor...");

            var users = await _context.Users.ToListAsync();
            var units = await _context.Units.ToListAsync();
            var properties = await _context.Properties.ToListAsync();
            var policies = await _context.Policies.ToListAsync();
            var reservations = new List<Reservation>();

            for (int i = 0; i < 200; i++)
            {
                var unit = _faker.PickRandom(units);
                var property = properties.First(p => p.Id == unit.PropertyId);
                var checkIn = _faker.Date.Between(DateTime.Now.AddDays(-30), DateTime.Now.AddDays(30));
                var checkOut = checkIn.AddDays(_faker.Random.Int(1, 14));
                var nights = (int)(checkOut - checkIn).TotalDays;

                var reservation = new Reservation
                {
                    CreatedDate = DateTime.UtcNow,
                    CreatorUserId = _adminUserId ?? Guid.Empty,
                    CreatorIP = "127.0.0.1",
                    UserId = _faker.PickRandom(users).Id,
                    UnitId = unit.Id,
                    PropertyId = property.Id,
                    PolicyId = _faker.PickRandom(policies).Id,
                    CheckInDate = checkIn,
                    CheckOutDate = checkOut,
                    GuestCount = _faker.Random.Int(1, 8),
                    Status = _faker.PickRandom<ReservationStatus>(),
                    TotalPrice = unit.Price * nights,
                    SpecialRequests = _faker.Lorem.Sentence(),
                    IsActive = true
                };

                reservations.Add(reservation);
            }

            _context.Reservations.AddRange(reservations);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} rezervasyon oluşturuldu", reservations.Count);
        }

        private async Task SeedReservationPriceBreakdownsAsync()
        {
            _logger.LogInformation("💰 Rezervasyon fiyat detayları oluşturuluyor...");

            var reservations = await _context.Reservations.ToListAsync();
            var priceBreakdowns = new List<ReservationPriceBreakdown>();

            foreach (var reservation in reservations)
            {
                var breakdowns = new Faker<ReservationPriceBreakdown>("tr")
                    .RuleFor(rpb => rpb.ReservationId, reservation.Id)
                    .RuleFor(rpb => rpb.LineType, f => f.PickRandom<PriceBreakdownType>())
                    .RuleFor(rpb => rpb.Name, f => f.PickRandom("Konaklama", "Temizlik Ücreti", "Güvenlik Depozitosu", "Vergi", "Hizmet Bedeli"))
                    .RuleFor(rpb => rpb.UnitPrice, f => f.Random.Decimal(50, 500))
                    .RuleFor(rpb => rpb.Qty, f => f.Random.Int(1, 5))
                    .RuleFor(rpb => rpb.Total, (f, rpb) => rpb.UnitPrice * rpb.Qty)
                    .RuleFor(rpb => rpb.IsActive, f => true)
                    .Generate(_faker.Random.Int(2, 5));

                priceBreakdowns.AddRange(breakdowns);
            }

            _context.ReservationPriceBreakdowns.AddRange(priceBreakdowns);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} rezervasyon fiyat detayı oluşturuldu", priceBreakdowns.Count);
        }

        private async Task SeedPaymentsAsync()
        {
            _logger.LogInformation("💳 Ödemeler oluşturuluyor...");

            var reservations = await _context.Reservations.ToListAsync();
            var payments = new List<Payment>();

            foreach (var reservation in reservations)
            {
                var reservationPayments = new Faker<Payment>("tr")
                    .RuleFor(p => p.ReservationId, reservation.Id)
                    .RuleFor(p => p.Amount, f => f.Random.Decimal(100, reservation.TotalPrice))
                    .RuleFor(p => p.Method, f => f.PickRandom<PaymentMethod>())
                    .RuleFor(p => p.Status, f => f.PickRandom<PaymentStatus>())
                    .RuleFor(p => p.TransactionRef, f => f.Random.AlphaNumeric(20))
                    .RuleFor(p => p.PaidAt, f => f.Date.Between(reservation.CheckInDate.AddDays(-30), reservation.CheckInDate))
                    .RuleFor(p => p.IsActive, f => true)
                    .Generate(_faker.Random.Int(1, 3));

                payments.AddRange(reservationPayments);
            }

            _context.Payments.AddRange(payments);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} ödeme oluşturuldu", payments.Count);
        }

        private async Task SeedPagesAsync()
        {
            _logger.LogInformation("📄 Sayfalar oluşturuluyor...");

            // Mevcut sayfaları kontrol et
            var existingPages = await _context.Pages.ToListAsync();
            if (existingPages.Any())
            {
                _logger.LogInformation("ℹ️ Sayfalar zaten mevcut, atlanıyor...");
                return;
            }

            var pageTitles = new[] { "Hakkımızda", "İletişim", "Gizlilik Politikası", "Kullanım Şartları", "SSS", "Kariyer", "Basın", "Yatırımcı İlişkileri" };
            var pages = new List<Page>();

            foreach (var title in pageTitles)
            {
                var slug = title.ToLowerInvariant()
                    .Replace(" ", "-")
                    .Replace("ı", "i")
                    .Replace("ğ", "g")
                    .Replace("ü", "u")
                    .Replace("ş", "s")
                    .Replace("ö", "o")
                    .Replace("ç", "c");

                var page = new Page
                {
                    Title = title,
                    Slug = slug,
                    Content = new Faker("tr").Lorem.Paragraphs(5),
                    MetaTitle = new Faker("tr").Lorem.Sentence(3),
                    MetaDescription = new Faker("tr").Lorem.Sentence(),
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    CreatorUserId = Guid.Empty,
                    CreatorIP = null
                };

                pages.Add(page);
            }

            _context.Pages.AddRange(pages);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} sayfa oluşturuldu", pages.Count);
        }

        private async Task SeedSeoContentsAsync()
        {
            _logger.LogInformation("🔍 SEO içerikleri oluşturuluyor...");

            var languages = await _context.Languages.ToListAsync();
            var seoContents = new List<SeoContent>();

            foreach (var language in languages)
            {
                var languageSeoContents = new Faker<SeoContent>("tr")
                    .RuleFor(sc => sc.LanguageId, language.Id)
                    .RuleFor(sc => sc.Key, f => f.PickRandom("home_title", "home_description", "about_title", "contact_title", "search_title"))
                    .RuleFor(sc => sc.Title, f => f.Lorem.Sentence(3))
                    .RuleFor(sc => sc.Content, f => f.Lorem.Sentence())
                    .RuleFor(sc => sc.Section, f => f.PickRandom<SeoSection>())
                    .RuleFor(sc => sc.IsActive, f => true)
                    .Generate(5);

                seoContents.AddRange(languageSeoContents);
            }

            _context.SeoContents.AddRange(seoContents);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} SEO içeriği oluşturuldu", seoContents.Count);
        }

        private async Task SeedBlogCategoriesAsync()
        {
            _logger.LogInformation("📝 Blog kategorileri oluşturuluyor...");

            // Mevcut kategorileri kontrol et
            var existingCategories = await _context.BlogCategories.ToListAsync();
            if (existingCategories.Any())
            {
                _logger.LogInformation("ℹ️ Blog kategorileri zaten mevcut, atlanıyor...");
                return;
            }

            var categoryNames = new[] { "Seyahat", "Konaklama", "Yemek", "Aktiviteler", "Kültür", "Doğa", "Spor", "Sanat" };
            var blogCategories = new List<BlogCategory>();

            foreach (var name in categoryNames)
            {
                var slug = name.ToLowerInvariant()
                    .Replace("ı", "i")
                    .Replace("ğ", "g")
                    .Replace("ü", "u")
                    .Replace("ş", "s")
                    .Replace("ö", "o")
                    .Replace("ç", "c");

                var category = new BlogCategory
                {
                    Name = name,
                    Slug = slug,
                    Description = new Faker("tr").Lorem.Sentence(),
                    Color = new Faker().Internet.Color(),
                    IconUrl = new Faker().Image.PicsumUrl(64, 64),
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    CreatorUserId = Guid.Empty,
                    CreatorIP = null
                };

                blogCategories.Add(category);
            }

            _context.BlogCategories.AddRange(blogCategories);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} blog kategorisi oluşturuldu", blogCategories.Count);
        }

        private async Task SeedBlogPostsAsync()
        {
            _logger.LogInformation("📰 Blog yazıları oluşturuluyor...");

            // Mevcut blog yazılarını kontrol et
            var existingPosts = await _context.BlogPosts.ToListAsync();
            if (existingPosts.Any())
            {
                _logger.LogInformation("ℹ️ Blog yazıları zaten mevcut, atlanıyor...");
                return;
            }

            var blogCategories = await _context.BlogCategories.ToListAsync();
            var blogPosts = new List<BlogPost>();

            for (int i = 0; i < 50; i++)
            {
                var blogPost = _blogPostFaker.Generate();
                blogPost.CategoryId = _faker.PickRandom(blogCategories).Id;
                blogPost.AuthorId = _faker.PickRandom(await _context.Users.ToListAsync()).Id;
                blogPosts.Add(blogPost);
            }

            _context.BlogPosts.AddRange(blogPosts);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} blog yazısı oluşturuldu", blogPosts.Count);
        }

        private async Task SeedBlogCommentsAsync()
        {
            _logger.LogInformation("💬 Blog yorumları oluşturuluyor...");

            var blogPosts = await _context.BlogPosts.ToListAsync();
            var users = await _context.Users.ToListAsync();
            var blogComments = new List<BlogComment>();

            foreach (var blogPost in blogPosts)
            {
                var postComments = new Faker<BlogComment>("tr")
                    .RuleFor(bc => bc.BlogPostId, blogPost.Id)
                    .RuleFor(bc => bc.UserId, f => _faker.PickRandom(users).Id)
                    .RuleFor(bc => bc.Content, f => f.Lorem.Sentence())
                    .RuleFor(bc => bc.IsApproved, f => f.Random.Bool())
                    .RuleFor(bc => bc.IsActive, f => true)
                    .Generate(_faker.Random.Int(0, 5));

                blogComments.AddRange(postComments);
            }

            _context.BlogComments.AddRange(blogComments);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} blog yorumu oluşturuldu", blogComments.Count);
        }

        private async Task SeedBlogTagsAsync()
        {
            _logger.LogInformation("🏷️ Blog etiketleri oluşturuluyor...");

            // Mevcut etiketleri kontrol et
            var existingTags = await _context.BlogTags.ToListAsync();
            if (existingTags.Any())
            {
                _logger.LogInformation("ℹ️ Blog etiketleri zaten mevcut, atlanıyor...");
                return;
            }

            var tagNames = new[] { "Seyahat", "Konaklama", "Yemek", "Aktiviteler", "Kültür", "Doğa", "Tarih", "Sanat", "Müzik", "Spor", "Tatil", "Macera", "Şehir", "Köy", "Deniz" };
            var blogTags = new List<BlogTag>();

            foreach (var name in tagNames)
            {
                var slug = name.ToLowerInvariant()
                    .Replace("ı", "i")
                    .Replace("ğ", "g")
                    .Replace("ü", "u")
                    .Replace("ş", "s")
                    .Replace("ö", "o")
                    .Replace("ç", "c");

                var tag = new BlogTag
                {
                    Name = name,
                    Slug = slug,
                    Color = new Faker().PickRandom("#6c757d", "#007bff", "#28a745", "#dc3545", "#ffc107", "#17a2b8"),
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    CreatorUserId = Guid.Empty,
                    CreatorIP = null
                };

                blogTags.Add(tag);
            }

            _context.BlogTags.AddRange(blogTags);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} blog etiketi oluşturuldu", blogTags.Count);
        }

        private async Task SeedPageCommentsAsync()
        {
            _logger.LogInformation("💬 Sayfa yorumları oluşturuluyor...");

            // Mevcut sayfa yorumlarını kontrol et
            var existingComments = await _context.PageComments.ToListAsync();
            if (existingComments.Any())
            {
                _logger.LogInformation("ℹ️ Sayfa yorumları zaten mevcut, atlanıyor...");
                return;
            }

            var pages = await _context.Pages.ToListAsync();
            var users = await _context.Users.ToListAsync();
            var pageComments = new List<PageComment>();

            foreach (var page in pages)
            {
                var pagePageComments = new Faker<PageComment>("tr")
                    .RuleFor(pc => pc.PageId, page.Id)
                    .RuleFor(pc => pc.UserId, f => _faker.PickRandom(users).Id)
                    .RuleFor(pc => pc.Name, f => f.Name.FullName())
                    .RuleFor(pc => pc.Email, f => f.Internet.Email())
                    .RuleFor(pc => pc.Content, f => f.Lorem.Sentence())
                    .RuleFor(pc => pc.IsApproved, f => f.Random.Bool())
                    .RuleFor(pc => pc.IsActive, f => true)
                    .RuleFor(pc => pc.CreatedDate, f => DateTime.UtcNow)
                    .RuleFor(pc => pc.CreatorUserId, f => Guid.Empty)
                    .RuleFor(pc => pc.CreatorIP, f => (string?)null)
                    .RuleFor(pc => pc.DisplayOrder, f => 0)
                    .Generate(_faker.Random.Int(0, 3));

                pageComments.AddRange(pagePageComments);
            }

            _context.PageComments.AddRange(pageComments);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} sayfa yorumu oluşturuldu", pageComments.Count);
        }

        private async Task SeedReviewsAsync()
        {
            _logger.LogInformation("⭐ Değerlendirmeler oluşturuluyor...");

            var properties = await _context.Properties.ToListAsync();
            var users = await _context.Users.ToListAsync();
            var reviews = new List<Review>();

            foreach (var property in properties)
            {
                var propertyReviews = new Faker<Review>("tr")
                    .RuleFor(r => r.PropertyId, property.Id)
                    .RuleFor(r => r.UserId, f => _faker.PickRandom(users).Id)
                    .RuleFor(r => r.Rating, f => f.Random.Int(1, 5))
                    .RuleFor(r => r.Title, f => f.Lorem.Sentence(3))
                    .RuleFor(r => r.Content, f => f.Lorem.Paragraph())
                    .RuleFor(r => r.IsApproved, f => f.Random.Bool())
                    .RuleFor(r => r.IsActive, f => true)
                    .Generate(_faker.Random.Int(0, 8));

                reviews.AddRange(propertyReviews);
            }

            _context.Reviews.AddRange(reviews);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} değerlendirme oluşturuldu", reviews.Count);
        }

        private async Task SeedQuestionsAsync()
        {
            _logger.LogInformation("❓ Sorular oluşturuluyor...");

            var properties = await _context.Properties.ToListAsync();
            var users = await _context.Users.ToListAsync();
            var questions = new List<Question>();

            foreach (var property in properties)
            {
                var propertyQuestions = new Faker<Question>("tr")
                    .RuleFor(q => q.PropertyId, property.Id)
                    .RuleFor(q => q.UserId, f => _faker.PickRandom(users).Id)
                    .RuleFor(q => q.QuestionText, f => f.Lorem.Sentence())
                    .RuleFor(q => q.AnswerText, f => f.Lorem.Sentence())
                    .RuleFor(q => q.IsAnswered, f => f.Random.Bool())
                    .RuleFor(q => q.IsActive, f => true)
                    .Generate(_faker.Random.Int(0, 5));

                questions.AddRange(propertyQuestions);
            }

            _context.Questions.AddRange(questions);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} soru oluşturuldu", questions.Count);
        }

        private async Task SeedFavoritesAsync()
        {
            _logger.LogInformation("❤️ Favoriler oluşturuluyor...");

            // Mevcut favorileri kontrol et
            var existingFavorites = await _context.Favorites.ToListAsync();
            if (existingFavorites.Any())
            {
                _logger.LogInformation("ℹ️ Favoriler zaten mevcut, atlanıyor...");
                return;
            }

            var properties = await _context.Properties.ToListAsync();
            var users = await _context.Users.ToListAsync();
            var favorites = new List<Favorite>();
            var usedCombinations = new HashSet<(Guid UserId, Guid PropertyId)>();

            foreach (var user in users)
            {
                var userFavorites = new Faker<Favorite>("tr")
                    .RuleFor(f => f.UserId, user.Id)
                    .RuleFor(f => f.PropertyId, f => _faker.PickRandom(properties).Id)
                    .RuleFor(f => f.Notes, f => f.Lorem.Sentence())
                    .RuleFor(f => f.IsActive, f => true)
                    .RuleFor(f => f.CreatedDate, f => DateTime.UtcNow)
                    .RuleFor(f => f.CreatorUserId, f => Guid.Empty)
                    .RuleFor(f => f.CreatorIP, f => (string?)null)
                    .RuleFor(f => f.DisplayOrder, f => 0)
                    .Generate(_faker.Random.Int(0, 10));

                // Duplicate kontrolü yap
                foreach (var favorite in userFavorites)
                {
                    var combination = (favorite.UserId, favorite.PropertyId);
                    if (!usedCombinations.Contains(combination))
                    {
                        usedCombinations.Add(combination);
                        favorites.Add(favorite);
                    }
                }
            }

            _context.Favorites.AddRange(favorites);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} favori oluşturuldu", favorites.Count);
        }

        private async Task SeedMessagesAsync()
        {
            _logger.LogInformation("💬 Mesajlar oluşturuluyor...");

            // Mevcut mesajları kontrol et
            var existingMessages = await _context.Messages.ToListAsync();
            if (existingMessages.Any())
            {
                _logger.LogInformation("ℹ️ Mesajlar zaten mevcut, atlanıyor...");
                return;
            }

            var users = await _context.Users.ToListAsync();
            if (users.Count < 2)
            {
                _logger.LogWarning("⚠️ Mesaj oluşturmak için en az 2 kullanıcı gerekli, atlanıyor...");
                return;
            }

            var messages = new List<Message>();

            for (int i = 0; i < 100; i++)
            {
                var sender = _faker.PickRandom(users);
                var receiver = _faker.PickRandom(users.Where(u => u.Id != sender.Id));

                var message = new Message
                {
                    SenderId = sender.Id,
                    ReceiverId = receiver.Id,
                    Subject = _faker.Lorem.Sentence(3),
                    Content = _faker.Lorem.Paragraph(),
                    IsRead = _faker.Random.Bool(),
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    CreatorUserId = Guid.Empty,
                    CreatorIP = null
                };

                messages.Add(message);
            }

            _context.Messages.AddRange(messages);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} mesaj oluşturuldu", messages.Count);
        }

        private async Task SeedDisputesAsync()
        {
            _logger.LogInformation("⚖️ Anlaşmazlıklar oluşturuluyor...");

            var reservations = await _context.Reservations.ToListAsync();
            var users = await _context.Users.ToListAsync();
            var disputes = new List<Dispute>();

            foreach (var reservation in reservations.Take(20))
            {
                var dispute = new Faker<Dispute>("tr")
                    .RuleFor(d => d.ReservationId, reservation.Id)
                    .RuleFor(d => d.ComplainantId, f => _faker.PickRandom(users).Id)
                    .RuleFor(d => d.Title, f => f.Lorem.Sentence(3))
                    .RuleFor(d => d.Description, f => f.Lorem.Paragraph())
                    .RuleFor(d => d.Status, f => f.PickRandom<DisputeStatus>())
                    .RuleFor(d => d.Priority, f => f.PickRandom<DisputePriority>())
                    .RuleFor(d => d.IsActive, f => true)
                    .Generate();

                disputes.Add(dispute);
            }

            _context.Disputes.AddRange(disputes);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} anlaşmazlık oluşturuldu", disputes.Count);
        }

        private async Task SeedCouponsAsync()
        {
            _logger.LogInformation("🎫 Kuponlar oluşturuluyor...");

            var coupons = _couponFaker.Generate(20);
            _context.Coupons.AddRange(coupons);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} kupon oluşturuldu", coupons.Count);
        }

        private async Task SeedCouponUsagesAsync()
        {
            _logger.LogInformation("🎫 Kupon kullanımları oluşturuluyor...");

            var coupons = await _context.Coupons.ToListAsync();
            var users = await _context.Users.ToListAsync();
            var couponUsages = new List<CouponUsage>();

            foreach (var coupon in coupons.Take(10))
            {
                var reservations = await _context.Reservations.ToListAsync();
                var usage = new Faker<CouponUsage>("tr")
                    .RuleFor(cu => cu.CouponId, coupon.Id)
                    .RuleFor(cu => cu.UserId, f => _faker.PickRandom(users).Id)
                    .RuleFor(cu => cu.ReservationId, f => _faker.PickRandom(reservations).Id)
                    .RuleFor(cu => cu.UsedAt, f => f.Date.Past())
                    .RuleFor(cu => cu.DiscountAmount, f => f.Random.Decimal(10, coupon.Value))
                    .RuleFor(cu => cu.IsActive, f => true)
                    .Generate(_faker.Random.Int(1, 5));

                couponUsages.AddRange(usage);
            }

            _context.CouponUsages.AddRange(couponUsages);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} kupon kullanımı oluşturuldu", couponUsages.Count);
        }

        private async Task SeedNotificationsAsync()
        {
            _logger.LogInformation("🔔 Bildirimler oluşturuluyor...");

            var users = await _context.Users.ToListAsync();
            var notifications = new List<Notification>();

            foreach (var user in users)
            {
                var userNotifications = _notificationFaker.Generate(_faker.Random.Int(5, 15));
                foreach (var notification in userNotifications)
                {
                    notification.UserId = user.Id;
                }
                notifications.AddRange(userNotifications);
            }

            _context.Notifications.AddRange(notifications);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} bildirim oluşturuldu", notifications.Count);
        }

        private async Task SeedSystemLogsAsync()
        {
            _logger.LogInformation("📊 Sistem logları oluşturuluyor...");

            var systemLogs = new Faker<SystemLog>("tr")
                .RuleFor(sl => sl.Level, f => f.PickRandom<Trimango.Data.Mssql.Enums.LogLevel>())
                .RuleFor(sl => sl.Message, f => f.Lorem.Sentence())
                .RuleFor(sl => sl.Source, f => f.PickRandom("API", "Database", "Authentication", "Payment", "Email"))
                .RuleFor(sl => sl.Action, f => f.PickRandom("Create", "Update", "Delete", "Login", "Logout", "Error"))
                .RuleFor(sl => sl.Exception, f => f.Lorem.Sentence())
                .RuleFor(sl => sl.IsActive, f => true)
                .Generate(100);

            _context.SystemLogs.AddRange(systemLogs);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} sistem logu oluşturuldu", systemLogs.Count);
        }

        private async Task SeedLocaleStringResourcesAsync()
        {
            _logger.LogInformation("🌐 Lokalizasyon kaynakları oluşturuluyor...");

            var languages = await _context.Languages.ToListAsync();
            var resources = new List<LocaleStringResource>();

            // Sistem genelinde kullanılan çeviri anahtarları ve değerleri
            var resourceData = new Dictionary<string, Dictionary<string, string>>
            {
                ["Common.Save"] = new Dictionary<string, string>
                {
                    ["tr"] = "Kaydet",
                    ["en"] = "Save",
                    ["de"] = "Speichern",
                    ["fr"] = "Enregistrer",
                    ["es"] = "Guardar"
                },
                ["Common.Cancel"] = new Dictionary<string, string>
                {
                    ["tr"] = "İptal",
                    ["en"] = "Cancel",
                    ["de"] = "Abbrechen",
                    ["fr"] = "Annuler",
                    ["es"] = "Cancelar"
                },
                ["Common.Delete"] = new Dictionary<string, string>
                {
                    ["tr"] = "Sil",
                    ["en"] = "Delete",
                    ["de"] = "Löschen",
                    ["fr"] = "Supprimer",
                    ["es"] = "Eliminar"
                },
                ["Common.Edit"] = new Dictionary<string, string>
                {
                    ["tr"] = "Düzenle",
                    ["en"] = "Edit",
                    ["de"] = "Bearbeiten",
                    ["fr"] = "Modifier",
                    ["es"] = "Editar"
                },
                ["Common.Add"] = new Dictionary<string, string>
                {
                    ["tr"] = "Ekle",
                    ["en"] = "Add",
                    ["de"] = "Hinzufügen",
                    ["fr"] = "Ajouter",
                    ["es"] = "Agregar"
                },
                ["Common.Search"] = new Dictionary<string, string>
                {
                    ["tr"] = "Ara",
                    ["en"] = "Search",
                    ["de"] = "Suchen",
                    ["fr"] = "Rechercher",
                    ["es"] = "Buscar"
                },
                ["Common.Filter"] = new Dictionary<string, string>
                {
                    ["tr"] = "Filtrele",
                    ["en"] = "Filter",
                    ["de"] = "Filter",
                    ["fr"] = "Filtrer",
                    ["es"] = "Filtrar"
                },
                ["Common.Clear"] = new Dictionary<string, string>
                {
                    ["tr"] = "Temizle",
                    ["en"] = "Clear",
                    ["de"] = "Löschen",
                    ["fr"] = "Effacer",
                    ["es"] = "Limpiar"
                },
                ["Common.Yes"] = new Dictionary<string, string>
                {
                    ["tr"] = "Evet",
                    ["en"] = "Yes",
                    ["de"] = "Ja",
                    ["fr"] = "Oui",
                    ["es"] = "Sí"
                },
                ["Common.No"] = new Dictionary<string, string>
                {
                    ["tr"] = "Hayır",
                    ["en"] = "No",
                    ["de"] = "Nein",
                    ["fr"] = "Non",
                    ["es"] = "No"
                },
                ["User.FirstName"] = new Dictionary<string, string>
                {
                    ["tr"] = "Ad",
                    ["en"] = "First Name",
                    ["de"] = "Vorname",
                    ["fr"] = "Prénom",
                    ["es"] = "Nombre"
                },
                ["User.LastName"] = new Dictionary<string, string>
                {
                    ["tr"] = "Soyad",
                    ["en"] = "Last Name",
                    ["de"] = "Nachname",
                    ["fr"] = "Nom de famille",
                    ["es"] = "Apellido"
                },
                ["User.Email"] = new Dictionary<string, string>
                {
                    ["tr"] = "E-posta",
                    ["en"] = "Email",
                    ["de"] = "E-Mail",
                    ["fr"] = "E-mail",
                    ["es"] = "Correo electrónico"
                },
                ["User.Password"] = new Dictionary<string, string>
                {
                    ["tr"] = "Şifre",
                    ["en"] = "Password",
                    ["de"] = "Passwort",
                    ["fr"] = "Mot de passe",
                    ["es"] = "Contraseña"
                },
                ["User.PhoneNumber"] = new Dictionary<string, string>
                {
                    ["tr"] = "Telefon Numarası",
                    ["en"] = "Phone Number",
                    ["de"] = "Telefonnummer",
                    ["fr"] = "Numéro de téléphone",
                    ["es"] = "Número de teléfono"
                },
                ["Validation.Required"] = new Dictionary<string, string>
                {
                    ["tr"] = "Bu alan zorunludur",
                    ["en"] = "This field is required",
                    ["de"] = "Dieses Feld ist erforderlich",
                    ["fr"] = "Ce champ est obligatoire",
                    ["es"] = "Este campo es obligatorio"
                },
                ["Validation.Email"] = new Dictionary<string, string>
                {
                    ["tr"] = "Geçerli bir e-posta adresi giriniz",
                    ["en"] = "Please enter a valid email address",
                    ["de"] = "Bitte geben Sie eine gültige E-Mail-Adresse ein",
                    ["fr"] = "Veuillez saisir une adresse e-mail valide",
                    ["es"] = "Por favor ingrese una dirección de correo electrónico válida"
                },
                ["Validation.MinLength"] = new Dictionary<string, string>
                {
                    ["tr"] = "En az {0} karakter olmalıdır",
                    ["en"] = "Must be at least {0} characters",
                    ["de"] = "Muss mindestens {0} Zeichen haben",
                    ["fr"] = "Doit contenir au moins {0} caractères",
                    ["es"] = "Debe tener al menos {0} caracteres"
                },
                ["Validation.MaxLength"] = new Dictionary<string, string>
                {
                    ["tr"] = "En fazla {0} karakter olmalıdır",
                    ["en"] = "Must be at most {0} characters",
                    ["de"] = "Darf höchstens {0} Zeichen haben",
                    ["fr"] = "Doit contenir au maximum {0} caractères",
                    ["es"] = "Debe tener como máximo {0} caracteres"
                },
                ["Auth.Login"] = new Dictionary<string, string>
                {
                    ["tr"] = "Giriş Yap",
                    ["en"] = "Login",
                    ["de"] = "Anmelden",
                    ["fr"] = "Se connecter",
                    ["es"] = "Iniciar sesión"
                },
                ["Auth.Register"] = new Dictionary<string, string>
                {
                    ["tr"] = "Kayıt Ol",
                    ["en"] = "Register",
                    ["de"] = "Registrieren",
                    ["fr"] = "S'inscrire",
                    ["es"] = "Registrarse"
                },
                ["Auth.Logout"] = new Dictionary<string, string>
                {
                    ["tr"] = "Çıkış Yap",
                    ["en"] = "Logout",
                    ["de"] = "Abmelden",
                    ["fr"] = "Se déconnecter",
                    ["es"] = "Cerrar sesión"
                },
                ["Auth.InvalidCredentials"] = new Dictionary<string, string>
                {
                    ["tr"] = "Geçersiz kullanıcı adı veya şifre",
                    ["en"] = "Invalid username or password",
                    ["de"] = "Ungültiger Benutzername oder Passwort",
                    ["fr"] = "Nom d'utilisateur ou mot de passe invalide",
                    ["es"] = "Nombre de usuario o contraseña inválidos"
                },
                ["Property.Title"] = new Dictionary<string, string>
                {
                    ["tr"] = "Başlık",
                    ["en"] = "Title",
                    ["de"] = "Titel",
                    ["fr"] = "Titre",
                    ["es"] = "Título"
                },
                ["Property.Description"] = new Dictionary<string, string>
                {
                    ["tr"] = "Açıklama",
                    ["en"] = "Description",
                    ["de"] = "Beschreibung",
                    ["fr"] = "Description",
                    ["es"] = "Descripción"
                },
                ["Property.Price"] = new Dictionary<string, string>
                {
                    ["tr"] = "Fiyat",
                    ["en"] = "Price",
                    ["de"] = "Preis",
                    ["fr"] = "Prix",
                    ["es"] = "Precio"
                },
                ["Property.Capacity"] = new Dictionary<string, string>
                {
                    ["tr"] = "Kapasite",
                    ["en"] = "Capacity",
                    ["de"] = "Kapazität",
                    ["fr"] = "Capacité",
                    ["es"] = "Capacidad"
                },
                ["Reservation.CheckIn"] = new Dictionary<string, string>
                {
                    ["tr"] = "Giriş Tarihi",
                    ["en"] = "Check-in Date",
                    ["de"] = "Check-in Datum",
                    ["fr"] = "Date d'arrivée",
                    ["es"] = "Fecha de entrada"
                },
                ["Reservation.CheckOut"] = new Dictionary<string, string>
                {
                    ["tr"] = "Çıkış Tarihi",
                    ["en"] = "Check-out Date",
                    ["de"] = "Check-out Datum",
                    ["fr"] = "Date de départ",
                    ["es"] = "Fecha de salida"
                },
                ["Reservation.GuestCount"] = new Dictionary<string, string>
                {
                    ["tr"] = "Misafir Sayısı",
                    ["en"] = "Guest Count",
                    ["de"] = "Anzahl der Gäste",
                    ["fr"] = "Nombre d'invités",
                    ["es"] = "Número de huéspedes"
                },
                ["Error.NotFound"] = new Dictionary<string, string>
                {
                    ["tr"] = "Kayıt bulunamadı",
                    ["en"] = "Record not found",
                    ["de"] = "Datensatz nicht gefunden",
                    ["fr"] = "Enregistrement introuvable",
                    ["es"] = "Registro no encontrado"
                },
                ["Error.Unauthorized"] = new Dictionary<string, string>
                {
                    ["tr"] = "Bu işlem için yetkiniz bulunmamaktadır",
                    ["en"] = "You are not authorized for this operation",
                    ["de"] = "Sie sind nicht berechtigt für diese Operation",
                    ["fr"] = "Vous n'êtes pas autorisé pour cette opération",
                    ["es"] = "No está autorizado para esta operación"
                },
                ["Error.InternalServer"] = new Dictionary<string, string>
                {
                    ["tr"] = "Sunucu hatası oluştu",
                    ["en"] = "Internal server error occurred",
                    ["de"] = "Interner Serverfehler aufgetreten",
                    ["fr"] = "Erreur interne du serveur",
                    ["es"] = "Error interno del servidor"
                },
                ["Success.Created"] = new Dictionary<string, string>
                {
                    ["tr"] = "Kayıt başarıyla oluşturuldu",
                    ["en"] = "Record created successfully",
                    ["de"] = "Datensatz erfolgreich erstellt",
                    ["fr"] = "Enregistrement créé avec succès",
                    ["es"] = "Registro creado exitosamente"
                },
                ["Success.Updated"] = new Dictionary<string, string>
                {
                    ["tr"] = "Kayıt başarıyla güncellendi",
                    ["en"] = "Record updated successfully",
                    ["de"] = "Datensatz erfolgreich aktualisiert",
                    ["fr"] = "Enregistrement mis à jour avec succès",
                    ["es"] = "Registro actualizado exitosamente"
                },
                ["Success.Deleted"] = new Dictionary<string, string>
                {
                    ["tr"] = "Kayıt başarıyla silindi",
                    ["en"] = "Record deleted successfully",
                    ["de"] = "Datensatz erfolgreich gelöscht",
                    ["fr"] = "Enregistrement supprimé avec succès",
                    ["es"] = "Registro eliminado exitosamente"
                }
            };

            foreach (var language in languages)
            {
                var languageCode = language.UniqueSeoCode;
                
                foreach (var resourceKey in resourceData.Keys)
                {
                    if (resourceData[resourceKey].ContainsKey(languageCode))
                    {
                        var resource = new LocaleStringResource
                        {
                            LanguageId = language.Id,
                            ResourceName = resourceKey,
                            ResourceValue = resourceData[resourceKey][languageCode],
                            DisplayOrder = 0,
                            IsActive = true,
                            CreatedDate = DateTime.UtcNow
                        };
                        resources.Add(resource);
                    }
                }
            }

            _context.LocaleStringResources.AddRange(resources);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} lokalizasyon kaynağı oluşturuldu", resources.Count);
        }

        private async Task SeedLocalizedPropertiesAsync()
        {
            _logger.LogInformation("🌐 Lokalize özellikler oluşturuluyor...");

            var languages = await _context.Languages.ToListAsync();
            var localizedProperties = new List<LocalizedProperty>();

            // Property'ler için çoklu dil desteği
            var properties = await _context.Properties.Take(20).ToListAsync();
            foreach (var property in properties)
            {
                foreach (var language in languages)
                {
                    var languageCode = language.UniqueSeoCode;
                    
                    // Property başlığı için çeviriler
                    var titleTranslations = new Dictionary<string, string>
                    {
                        ["tr"] = $"Lüks {property.Title}",
                        ["en"] = $"Luxury {property.Title}",
                        ["de"] = $"Luxus {property.Title}",
                        ["fr"] = $"Luxe {property.Title}",
                        ["es"] = $"Lujo {property.Title}"
                    };

                    if (titleTranslations.ContainsKey(languageCode))
                    {
                        localizedProperties.Add(new LocalizedProperty
                        {
                            EntityId = property.Id,
                            LanguageId = language.Id,
                            LocaleKeyGroup = LocaleKeyGroup.Property,
                            LocaleKey = "Title",
                            LocaleValue = titleTranslations[languageCode],
                            IsActive = true,
                            CreatedDate = DateTime.UtcNow,
                            CreatorUserId = Guid.Empty,
                            CreatorIP = null,
                            DisplayOrder = 0
                        });
                    }

                    // Property açıklaması için çeviriler
                    var descriptionTranslations = new Dictionary<string, string>
                    {
                        ["tr"] = $"Bu konaklama tesisi {property.Description}",
                        ["en"] = $"This accommodation facility {property.Description}",
                        ["de"] = $"Diese Unterkunft {property.Description}",
                        ["fr"] = $"Cette installation d'hébergement {property.Description}",
                        ["es"] = $"Esta instalación de alojamiento {property.Description}"
                    };

                    if (descriptionTranslations.ContainsKey(languageCode))
                    {
                        localizedProperties.Add(new LocalizedProperty
                        {
                            EntityId = property.Id,
                            LanguageId = language.Id,
                            LocaleKeyGroup = LocaleKeyGroup.Property,
                            LocaleKey = "Description",
                            LocaleValue = descriptionTranslations[languageCode],
                            IsActive = true,
                            CreatedDate = DateTime.UtcNow,
                            CreatorUserId = Guid.Empty,
                            CreatorIP = null,
                            DisplayOrder = 0
                        });
                    }
                }
            }

            // PropertyType'lar için çoklu dil desteği
            var propertyTypes = await _context.PropertyTypes.ToListAsync();
            foreach (var propertyType in propertyTypes)
            {
                foreach (var language in languages)
                {
                    var languageCode = language.UniqueSeoCode;
                    
                    var nameTranslations = new Dictionary<string, string>
                    {
                        ["tr"] = propertyType.Name,
                        ["en"] = GetEnglishPropertyTypeName(propertyType.Name),
                        ["de"] = GetGermanPropertyTypeName(propertyType.Name),
                        ["fr"] = GetFrenchPropertyTypeName(propertyType.Name),
                        ["es"] = GetSpanishPropertyTypeName(propertyType.Name)
                    };

                    if (nameTranslations.ContainsKey(languageCode))
                    {
                        localizedProperties.Add(new LocalizedProperty
                        {
                            EntityId = propertyType.Id,
                            LanguageId = language.Id,
                            LocaleKeyGroup = LocaleKeyGroup.Property,
                            LocaleKey = "Name",
                            LocaleValue = nameTranslations[languageCode],
                            IsActive = true,
                            CreatedDate = DateTime.UtcNow,
                            CreatorUserId = Guid.Empty,
                            CreatorIP = null,
                            DisplayOrder = 0
                        });
                    }
                }
            }

            // Feature'lar için çoklu dil desteği
            var features = await _context.Features.Take(15).ToListAsync();
            foreach (var feature in features)
            {
                foreach (var language in languages)
                {
                    var languageCode = language.UniqueSeoCode;
                    
                    var nameTranslations = new Dictionary<string, string>
                    {
                        ["tr"] = feature.Name,
                        ["en"] = GetEnglishFeatureName(feature.Name),
                        ["de"] = GetGermanFeatureName(feature.Name),
                        ["fr"] = GetFrenchFeatureName(feature.Name),
                        ["es"] = GetSpanishFeatureName(feature.Name)
                    };

                    if (nameTranslations.ContainsKey(languageCode))
                    {
                        localizedProperties.Add(new LocalizedProperty
                        {
                            EntityId = feature.Id,
                            LanguageId = language.Id,
                            LocaleKeyGroup = LocaleKeyGroup.Feature,
                            LocaleKey = "Name",
                            LocaleValue = nameTranslations[languageCode],
                            IsActive = true,
                            CreatedDate = DateTime.UtcNow,
                            CreatorUserId = Guid.Empty,
                            CreatorIP = null,
                            DisplayOrder = 0
                        });
                    }
                }
            }

            // BlogPost'lar için çoklu dil desteği
            var blogPosts = await _context.BlogPosts.Take(10).ToListAsync();
            foreach (var blogPost in blogPosts)
            {
                foreach (var language in languages)
                {
                    var languageCode = language.UniqueSeoCode;
                    
                    var titleTranslations = new Dictionary<string, string>
                    {
                        ["tr"] = blogPost.Title,
                        ["en"] = $"Travel Guide: {blogPost.Title}",
                        ["de"] = $"Reiseführer: {blogPost.Title}",
                        ["fr"] = $"Guide de voyage: {blogPost.Title}",
                        ["es"] = $"Guía de viaje: {blogPost.Title}"
                    };

                    if (titleTranslations.ContainsKey(languageCode))
                    {
                        localizedProperties.Add(new LocalizedProperty
                        {
                            EntityId = blogPost.Id,
                            LanguageId = language.Id,
                            LocaleKeyGroup = LocaleKeyGroup.Blog,
                            LocaleKey = "Title",
                            LocaleValue = titleTranslations[languageCode],
                            IsActive = true,
                            CreatedDate = DateTime.UtcNow,
                            CreatorUserId = Guid.Empty,
                            CreatorIP = null,
                            DisplayOrder = 0
                        });
                    }
                }
            }

            _context.LocalizedProperties.AddRange(localizedProperties);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ {Count} lokalize özellik oluşturuldu", localizedProperties.Count);
        }

        // Yardımcı metodlar - PropertyType çevirileri
        private string GetEnglishPropertyTypeName(string turkishName)
        {
            return turkishName switch
            {
                "Villa" => "Villa",
                "Apartman" => "Apartment",
                "Ev" => "House",
                "Stüdyo" => "Studio",
                "Loft" => "Loft",
                "Çiftlik Evi" => "Farmhouse",
                "Yazlık" => "Summer House",
                "Köşk" => "Mansion",
                _ => turkishName
            };
        }

        private string GetGermanPropertyTypeName(string turkishName)
        {
            return turkishName switch
            {
                "Villa" => "Villa",
                "Apartman" => "Wohnung",
                "Ev" => "Haus",
                "Stüdyo" => "Studio",
                "Loft" => "Loft",
                "Çiftlik Evi" => "Bauernhaus",
                "Yazlık" => "Sommerhaus",
                "Köşk" => "Herrenhaus",
                _ => turkishName
            };
        }

        private string GetFrenchPropertyTypeName(string turkishName)
        {
            return turkishName switch
            {
                "Villa" => "Villa",
                "Apartman" => "Appartement",
                "Ev" => "Maison",
                "Stüdyo" => "Studio",
                "Loft" => "Loft",
                "Çiftlik Evi" => "Ferme",
                "Yazlık" => "Maison d'été",
                "Köşk" => "Manoir",
                _ => turkishName
            };
        }

        private string GetSpanishPropertyTypeName(string turkishName)
        {
            return turkishName switch
            {
                "Villa" => "Villa",
                "Apartman" => "Apartamento",
                "Ev" => "Casa",
                "Stüdyo" => "Estudio",
                "Loft" => "Loft",
                "Çiftlik Evi" => "Casa de campo",
                "Yazlık" => "Casa de verano",
                "Köşk" => "Mansión",
                _ => turkishName
            };
        }

        // Yardımcı metodlar - Feature çevirileri
        private string GetEnglishFeatureName(string turkishName)
        {
            return turkishName switch
            {
                "WiFi" => "WiFi",
                "Havuz" => "Pool",
                "Spa" => "Spa",
                "Fitness" => "Fitness",
                "Parking" => "Parking",
                "Balcony" => "Balcony",
                "Garden" => "Garden",
                "Sea View" => "Sea View",
                _ => turkishName
            };
        }

        private string GetGermanFeatureName(string turkishName)
        {
            return turkishName switch
            {
                "WiFi" => "WiFi",
                "Havuz" => "Pool",
                "Spa" => "Spa",
                "Fitness" => "Fitness",
                "Parking" => "Parkplatz",
                "Balcony" => "Balkon",
                "Garden" => "Garten",
                "Sea View" => "Meerblick",
                _ => turkishName
            };
        }

        private string GetFrenchFeatureName(string turkishName)
        {
            return turkishName switch
            {
                "WiFi" => "WiFi",
                "Havuz" => "Piscine",
                "Spa" => "Spa",
                "Fitness" => "Fitness",
                "Parking" => "Parking",
                "Balcony" => "Balcon",
                "Garden" => "Jardin",
                "Sea View" => "Vue sur mer",
                _ => turkishName
            };
        }

        private string GetSpanishFeatureName(string turkishName)
        {
            return turkishName switch
            {
                "WiFi" => "WiFi",
                "Havuz" => "Piscina",
                "Spa" => "Spa",
                "Fitness" => "Fitness",
                "Parking" => "Aparcamiento",
                "Balcony" => "Balcón",
                "Garden" => "Jardín",
                "Sea View" => "Vista al mar",
                _ => turkishName
            };
        }
    }
}