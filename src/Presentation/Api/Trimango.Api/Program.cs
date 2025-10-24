using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Serilog;
using FluentValidation.AspNetCore;
using StackExchange.Redis;
using Trimango.Mssql;
using Trimango.Data.Mssql;
using Maggsoft.Mssql.Extensions;
using Maggsoft.Mssql.Repository;
using Maggsoft.Core.IoC;
using Maggsoft.Framework.Extensions;
using Maggsoft.Framework.Middleware;
using Maggsoft.Services.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Maggsoft.Cache.MemoryCache;
using Maggsoft.Cache;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Maggsoft.Data.Extensions;
using AspNetCoreRateLimit;

using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Maggsoft.Framework.Middleware.ApiResponseMiddleware;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Trimango.Api.Middleware;
using Trimango.Api.Models;
using Trimango.Api.Utilities;
using Trimango.Mssql.Services.Interfaces;
using Trimango.Mssql.Services.Services;
using Trimango.Api.Services;
using Trimango.Data.Mssql.Entities;
using Trimango.Mssql.Services.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Database connection string'i appsettings'ten al ve environment variable'ı resolve et
var connectionString = EnvironmentHelper.ResolveEnvironmentVariable(builder.Configuration.GetConnectionString("DefaultConnection"));

// Debug için connection string'i yazdır
Console.WriteLine($"🔍 Connection String: {connectionString}");

if (string.IsNullOrEmpty(connectionString))
{
    Log.Fatal("❌ DefaultConnection connection string tanımlanmamış! Uygulama başlatılamıyor.");
    throw new InvalidOperationException("DefaultConnection connection string is required in appsettings.json.");
}


// Serilog konfigürasyonu - Tamamen manuel (placeholder sorununu önler)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.MSSqlServer(
        connectionString: connectionString,
        sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true,
            BatchPostingLimit = 100,
            BatchPeriod = TimeSpan.FromSeconds(2)
        },
        columnOptions: new Serilog.Sinks.MSSqlServer.ColumnOptions
        {
            AdditionalColumns = new System.Collections.ObjectModel.Collection<Serilog.Sinks.MSSqlServer.SqlColumn>
            {
                new("UserId", System.Data.SqlDbType.NVarChar, dataLength: 50),
                new("IPAddress", System.Data.SqlDbType.NVarChar, dataLength: 45)
            }
        })
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

// API Versioning konfigürasyonu
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"),
        new MediaTypeApiVersionReader("ver")
    );
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Swagger konfigürasyonu - API versioning ile entegre
var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
builder.Services.AddSwaggerGen(options =>
{
    // Her API versiyonu için Swagger dokümantasyonu oluştur
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerDoc(description.GroupName, new OpenApiInfo
        {
            Title = $"Trimango API {description.ApiVersion}",
            Version = description.ApiVersion.ToString(),
            Description = description.IsDeprecated
                ? "⚠️ Bu API versiyonu kullanımdan kaldırılmıştır. Lütfen yeni versiyonu kullanın."
                : "Tatil Konaklama Pazar Yeri Sistemi API",
            Contact = new OpenApiContact
            {
                Name = "Trimango Destek Ekibi",
                Email = "destek@trimango.com",
                Url = new Uri("https://trimango.com")
            },
            License = new OpenApiLicense
            {
                Name = "Özel Lisans",
                Url = new Uri("https://trimango.com/license")
            },
            TermsOfService = new Uri("https://trimango.com/terms")
        });
    }

    // JWT Bearer Authentication için Swagger konfigürasyonu
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header kullanarak Bearer token girişi. Örnek: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // XML comments dosyasını dahil et
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Database ve Migration konfigürasyonu
builder.Services
    .AddMssqlConfig<TrimangoDbContext>(connectionString, o =>
    {
        o.UseCompatibilityLevel(120);
        o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    }).AddFluentMigratorConfig(connectionString);

// Identity konfigürasyonu
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Password ayarları
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    // User ayarları
    options.User.RequireUniqueEmail = false; // Email tekil validasyonu kaldırıldı
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    // Lockout ayarları (varsayılan değerler)
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // Token ayarları
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
})
.AddEntityFrameworkStores<TrimangoDbContext>()
.AddRoleManager<RoleManager<ApplicationRole>>()
.AddDefaultTokenProviders()
.AddErrorDescriber<LocalizedIdentityErrorDescriber>();

// JWT Settings konfigürasyonu - Environment variable'ları resolve et
builder.Services.Configure<JwtSettings>(options =>
{
    var jwtSection = builder.Configuration.GetSection("JwtSettings");
    jwtSection.Bind(options);
    // JWT Secret Key'i environment variable'dan al ve geri yaz (güvenlik için)
    var secretKey = EnvironmentHelper.ResolveEnvironmentVariable(options.SecretKey ?? "");
    if (string.IsNullOrEmpty(secretKey))
    {
        Log.Fatal("❌ JWT_SECRET_KEY environment variable tanımlanmamış! Uygulama başlatılamıyor.");
        throw new InvalidOperationException("JWT_SECRET_KEY environment variable is required. Please set it in your environment or Azure App Service Configuration.");
    }
    options.SecretKey = secretKey;

});


// HttpContext Accessor
builder.Services.AddHttpContextAccessor();

// JWT Authentication konfigürasyonu - Resolve edilmiş JwtSettings'i kullan
var jwtSettings = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtSettings>>().Value;

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        // Production'da HTTPS zorunlu, Development'da opsiyonel (güvenlik)
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings?.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings?.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            // Token süre validasyonu
            RequireExpirationTime = true,
            ValidateTokenReplay = false
        };

        // SignalR için JWT token doğrulama
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });


// Seed Data Service
builder.Services.AddScoped<SeedDataService>();

// AutoMapper konfigürasyonu
builder.Services.AddAutoMapperConfig(p => p.AddProfile<MappingProfile>(), typeof(Program));

// FluentValidation konfigürasyonu
builder.Services.AddFluentValidationAutoValidation();

// Repository pattern konfigürasyonu
builder.Services.AddScoped(typeof(IMssqlRepository<>), typeof(Repository<>));

// Cache konfigürasyonu
builder.Services.AddMaggsoftDistributedMemoryCache(typeof(IService));

// Forwarded Headers (reverse proxy için)
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

// Rate Limiting konfigürasyonu
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddDistributedRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();


// Maggsoft IoC konfigürasyonu
builder.Services.RegisterAll<IService>();

// JWT Service
builder.Services.AddScoped<IJwtService, JwtService>();


// HTTP Context Accessor
builder.Services.AddHttpContextAccessor();

// Middleware'ler
builder.Services.AddExceptionHandler<ExceptionMiddleware>();
builder.Services.AddProblemDetails();

// CORS konfigürasyonu
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Response ignore header
builder.Services.AddGlobalResponseMiddlewareWithOptions(options =>
{
    options.IgnoreAcceptHeader = ["image/", "txt", "text/html"];
    options.UseCamelCase = true;

    options.OnMessageLocalization += async (sender, e) =>
    {
        try
        {
            var cache = e.HttpContext?.RequestServices?.GetService<ICache>();
            if (cache != null)
            {
                var xLanguage = e.HttpContext?.Request?.Headers["X-Language"].FirstOrDefault();
                var languageCode = !string.IsNullOrEmpty(xLanguage) ? xLanguage : "tr";

                var languageMapping = await cache.GetAsync<Dictionary<string, Guid>>("LanguageMapping", TimeSpan.FromDays(1), async () => []);

                if (languageMapping.TryGetValue(languageCode, out var languageId))
                {
                    var localizationCache = await cache.GetAsync<Dictionary<string, string>>("LocalizationCache", TimeSpan.FromDays(1), async () => new Dictionary<string, string>());

                     if (localizationCache.TryGetValue( $"{languageId}_{e.MessageKey}", out var localizedMessage))
                     {
                         e.LocalizedMessage = localizedMessage;
                         return;
                     }
                }
            }
        }
        catch (Exception)
        {
            // ignored
        }

        e.LocalizedMessage = e.DefaultMessage;
    };
});

builder.Services.AddIPFilter(options =>
{
    //builder.Configuration.GetSection("IPFilter").Bind(options);
    options.MaxRequestsPerMinute = 60;
    options.WhitelistedIPs = ["127.0.0.1", "::1"];
    //options.StrictMode = true;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // Her API versiyonu için Swagger endpoint'i ekle
    var versionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in versionProvider.ApiVersionDescriptions)
    {
        c.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            $"TrinkEmlak API {description.GroupName.ToUpperInvariant()}{(description.IsDeprecated ? " (Deprecated)" : "")}"
        );
    }
    c.RoutePrefix = string.Empty; // Swagger UI'ı root'ta göster
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
});


// Configure the HTTP request pipeline.
app.UseForwardedHeaders();
app.UseSerilogEnrichment();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAll");

// Security Headers Middleware - XSS, Clickjacking, MIME-type sniffing koruması
app.Use(async (context, next) =>
{
    // XSS (Cross-Site Scripting) koruması
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY"; // Clickjacking önleme
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";

    // Referrer Policy - Güvenli referrer bilgisi paylaşımı
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

    // Content Security Policy (CSP) - XSS önleme
    context.Response.Headers["Content-Security-Policy"] =
        "default-src 'self'; " +
        "img-src 'self' https://trinkemlak.blob.core.windows.net data:; " +
        "media-src 'self' https://trinkemlak.blob.core.windows.net; " +
        "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +  // Swagger için unsafe-inline gerekli
        "style-src 'self' 'unsafe-inline'; " +
        "font-src 'self' data:; " +
        "connect-src 'self' https://trinkemlak.blob.core.windows.net wss: ws:;"; // SignalR için wss:/ws: gerekli

    // HSTS (HTTP Strict Transport Security) - Production'da HTTPS zorunlu
    if (!builder.Environment.IsDevelopment() && !context.Request.Host.Host.Contains("localhost"))
    {
        context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains; preload";
    }

    // Permissions Policy (Feature Policy) - Tarayıcı özelliklerine erişim kısıtlama
    context.Response.Headers["Permissions-Policy"] =
        "geolocation=(), microphone=(), camera=(), payment=(), usb=()";

    await next();
});

app.UseMiddleware<ApiResponseMiddleware>();

app.UseIpRateLimiting();

app.UseAuthentication();
app.UseAuthorization();

// IP Filter
app.UseIPFilter();

Log.Information("🚀 Database migration başlatılıyor...");
try
{
    app.AddUpMigrate(); // Migration'lar otomatik çalıştırılacak
    Log.Information("Database migration'ları başarıyla tamamlandı");
}
catch (Exception ex)
{
    Log.Error(ex, "Database migration'ları çalıştırılırken hata oluştu");
    // Migration hatası durumunda uygulama çalışmaya devam etsin
}

// Seed data
var seedDataEnabled = builder.Configuration.GetValue<bool>("SeedData:Enabled");
if (seedDataEnabled)
{
    try
    {
        using var scope = app.Services.CreateScope();
        var seedDataService = scope.ServiceProvider.GetRequiredService<SeedDataService>();
        await seedDataService.SeedAllDataAsync();
        Log.Information("🌱 Seed data oluşturuldu!");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "❌ Seed data oluşturulurken hata oluştu!");
        // Seed data hatası uygulamayı durdurmaz, devam et
        Log.Information("🚀 API seed data olmadan çalışmaya devam ediyor...");
    }
}

// Localization cache initialization
try
{
    using var localizationScope = app.Services.CreateScope();
    var localizationService = localizationScope.ServiceProvider.GetRequiredService<ILocalizationService>();
    await localizationService.InitializeLocalizationCacheAsync();
    Log.Information("Localization cache başarıyla yüklendi");
}
catch (Exception ex)
{
    Log.Warning(ex, "Localization cache yüklenirken hata oluştu, uygulama devam ediyor");
}

app.MapControllers();

try
{
    Log.Information("Trimango API başlatılıyor...");

    // ConfigureRequestPipeline'ı başta çağır
    app.ConfigureRequestPipeline();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Trimango API başlatılırken hata oluştu");
}
finally
{
    Log.CloseAndFlush();
}
