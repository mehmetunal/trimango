using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Trimango.Data.Mssql.Entities;
using Trimango.Dto.Mssql.Auth;

namespace Trimango.Api.Services
{
    /// <summary>
    /// JWT Token servisi
    /// </summary>
    public interface IJwtService
    {
        Task<AuthResponseDto> GenerateTokenAsync(ApplicationUser user);
        Task<AuthResponseDto?> RefreshTokenAsync(string accessToken, string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken);
    }

    public class JwtService : IJwtService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtService> _logger;

        public JwtService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration,
            ILogger<JwtService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AuthResponseDto> GenerateTokenAsync(ApplicationUser user)
        {
            try
            {
                // Kullanıcı rollerini al
                var userRoles = await _userManager.GetRolesAsync(user);

                // Claims oluştur
                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Email, user.Email ?? ""),
                    new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                    new(ClaimTypes.GivenName, user.FirstName),
                    new(ClaimTypes.Surname, user.LastName),
                    new("FirstName", user.FirstName),
                    new("LastName", user.LastName),
                    new("IsSupplier", user.IsSupplier.ToString()),
                    new("EmailConfirmed", user.EmailConfirmed.ToString()),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                };

                // Rolleri ekle
                foreach (var role in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                // JWT ayarları
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey bulunamadı");
                var issuer = jwtSettings["Issuer"] ?? "Trimango";
                var audience = jwtSettings["Audience"] ?? "TrimangoUsers";
                var expirationMinutes = int.Parse(jwtSettings["AccessTokenExpirationMinutes"] ?? "60");

                // Token oluştur
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                    signingCredentials: credentials
                );

                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
                var refreshToken = GenerateRefreshToken();

                // Refresh token'ı kullanıcıya kaydet (basit implementasyon)
                // Gerçek uygulamada Redis veya database'de saklanmalı
                user.LastLoginDate = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                return new AuthResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = token.ValidTo,
                    TokenType = "Bearer",
                    User = new UserInfoDto
                    {
                        Id = user.Id,
                        Email = user.Email ?? "",
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        ProfilePictureUrl = user.ProfilePictureUrl,
                        IsSupplier = user.IsSupplier,
                        EmailConfirmed = user.EmailConfirmed,
                        CreatedDate = user.CreatedDate,
                        LastLoginDate = user.LastLoginDate,
                        Roles = userRoles.ToList()
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token oluşturulurken hata oluştu: {UserId}", user.Id);
                throw;
            }
        }

        public async Task<AuthResponseDto?> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            try
            {
                // Access token'ı validate et
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey bulunamadı");

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = false, // Expired token'ları da kabul et
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(accessToken, validationParameters, out var validatedToken);
                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return null;
                }

                // Kullanıcıyı bul
                var user = await _userManager.FindByIdAsync(userIdClaim);
                if (user == null || !user.IsActive || user.IsDeleted)
                {
                    return null;
                }

                // Refresh token'ı validate et (basit implementasyon)
                // Gerçek uygulamada Redis veya database'den kontrol edilmeli
                if (!IsValidRefreshToken(refreshToken))
                {
                    return null;
                }

                // Yeni token oluştur
                return await GenerateTokenAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token yenilenirken hata oluştu");
                return null;
            }
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            try
            {
                // Refresh token'ı iptal et (basit implementasyon)
                // Gerçek uygulamada Redis veya database'den silinmeli
                _logger.LogInformation("Refresh token iptal edildi: {RefreshToken}", refreshToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token iptal edilirken hata oluştu");
                return false;
            }
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        private bool IsValidRefreshToken(string refreshToken)
        {
            // Basit implementasyon - gerçek uygulamada Redis veya database'den kontrol edilmeli
            return !string.IsNullOrEmpty(refreshToken) && refreshToken.Length > 10;
        }
    }
}
