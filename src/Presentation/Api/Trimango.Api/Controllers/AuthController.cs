using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Trimango.Api.Services;
using Trimango.Data.Mssql.Entities;
using Trimango.Dto.Mssql.Auth;
using Trimango.Dto.Mssql.User;
using Trimango.Mssql.Services.Interfaces;

namespace Trimango.Api.Controllers
{
    /// <summary>
    /// Authentication Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService,
            IUserService userService,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Kullanıcı girişi
        /// </summary>
        /// <param name="dto">Giriş bilgileri</param>
        /// <returns>JWT token ve kullanıcı bilgileri</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Kullanıcıyı bul
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                {
                    _logger.LogWarning("Giriş başarısız: Email bulunamadı - {Email}", dto.Email);
                    return Unauthorized(new { message = "Email veya şifre hatalı" });
                }

                // Kullanıcı aktif mi kontrol et
                if (!user.IsActive || user.IsDeleted)
                {
                    _logger.LogWarning("Giriş başarısız: Kullanıcı aktif değil - {Email}", dto.Email);
                    return Unauthorized(new { message = "Hesabınız aktif değil" });
                }

                // Şifre kontrolü
                var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("Giriş başarısız: Şifre hatalı - {Email}", dto.Email);
                    return Unauthorized(new { message = "Email veya şifre hatalı" });
                }

                // Hesap kilitli mi kontrol et
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Giriş başarısız: Hesap kilitli - {Email}", dto.Email);
                    return Unauthorized(new { message = "Hesabınız geçici olarak kilitlenmiştir" });
                }

                // Token oluştur
                var authResponse = await _jwtService.GenerateTokenAsync(user);

                _logger.LogInformation("Başarılı giriş: {Email}", dto.Email);

                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Giriş işlemi sırasında hata oluştu: {Email}", dto.Email);
                return StatusCode(500, new { message = "Sunucu hatası oluştu" });
            }
        }

        /// <summary>
        /// Kullanıcı kaydı
        /// </summary>
        /// <param name="dto">Kayıt bilgileri</param>
        /// <returns>JWT token ve kullanıcı bilgileri</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Email zaten var mı kontrol et
                var existingUser = await _userManager.FindByEmailAsync(dto.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("Kayıt başarısız: Email zaten kullanımda - {Email}", dto.Email);
                    return Conflict(new { message = "Bu email adresi zaten kullanımda" });
                }

                // Kullanıcı oluştur
                var result = await _userService.CreateUserAsync(new CreateUserDto
                {
                    Email = dto.Email,
                    Password = dto.Password,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    PhoneNumber = dto.PhoneNumber,
                    IsSupplier = dto.IsSupplier
                });

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Kayıt başarısız: {Error}", result.ErrorMessage);
                    return BadRequest(new { message = result.ErrorMessage });
                }

                // Token oluştur
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                {
                    return StatusCode(500, new { message = "Kullanıcı oluşturuldu ancak token oluşturulamadı" });
                }

                var authResponse = await _jwtService.GenerateTokenAsync(user);

                _logger.LogInformation("Başarılı kayıt: {Email}", dto.Email);

                return CreatedAtAction(nameof(GetProfile), new { }, authResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kayıt işlemi sırasında hata oluştu: {Email}", dto.Email);
                return StatusCode(500, new { message = "Sunucu hatası oluştu" });
            }
        }

        /// <summary>
        /// Token yenileme
        /// </summary>
        /// <param name="dto">Token bilgileri</param>
        /// <returns>Yeni JWT token</returns>
        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var authResponse = await _jwtService.RefreshTokenAsync(dto.AccessToken, dto.RefreshToken);
                if (authResponse == null)
                {
                    _logger.LogWarning("Token yenileme başarısız");
                    return Unauthorized(new { message = "Geçersiz token" });
                }

                _logger.LogInformation("Token başarıyla yenilendi");
                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token yenileme işlemi sırasında hata oluştu");
                return StatusCode(500, new { message = "Sunucu hatası oluştu" });
            }
        }

        /// <summary>
        /// Çıkış yapma
        /// </summary>
        /// <returns>Başarı mesajı</returns>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDto? dto = null)
        {
            try
            {
                // Refresh token'ı iptal et
                if (dto != null && !string.IsNullOrEmpty(dto.RefreshToken))
                {
                    await _jwtService.RevokeTokenAsync(dto.RefreshToken);
                }

                // Sign out
                await _signInManager.SignOutAsync();

                _logger.LogInformation("Başarılı çıkış");
                return Ok(new { message = "Başarıyla çıkış yapıldı" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Çıkış işlemi sırasında hata oluştu");
                return StatusCode(500, new { message = "Sunucu hatası oluştu" });
            }
        }

        /// <summary>
        /// Kullanıcı profil bilgileri
        /// </summary>
        /// <returns>Kullanıcı bilgileri</returns>
        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(typeof(UserInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(new { message = "Geçersiz token" });
                }

                var result = await _userService.GetUserByIdAsync(userId);
                if (!result.IsSuccess || result.Data == null)
                {
                    return NotFound(new { message = "Kullanıcı bulunamadı" });
                }

                var user = await _userManager.FindByIdAsync(userIdClaim);
                if (user == null)
                {
                    return NotFound(new { message = "Kullanıcı bulunamadı" });
                }

                var userRoles = await _userManager.GetRolesAsync(user);

                var userInfo = new UserInfoDto
                {
                    Id = result.Data.Id,
                    Email = result.Data.Email,
                    FirstName = result.Data.FirstName,
                    LastName = result.Data.LastName,
                    PhoneNumber = result.Data.PhoneNumber,
                    ProfilePictureUrl = result.Data.ProfilePictureUrl,
                    IsSupplier = result.Data.IsSupplier,
                    EmailConfirmed = result.Data.EmailConfirmed,
                    CreatedDate = result.Data.CreatedDate,
                    LastLoginDate = result.Data.LastLoginDate,
                    Roles = userRoles.ToList()
                };

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Profil bilgileri alınırken hata oluştu");
                return StatusCode(500, new { message = "Sunucu hatası oluştu" });
            }
        }

        /// <summary>
        /// Şifre değiştirme
        /// </summary>
        /// <param name="dto">Şifre değiştirme bilgileri</param>
        /// <returns>Başarı mesajı</returns>
        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new { message = "Geçersiz token" });
                }

                var user = await _userManager.FindByIdAsync(userIdClaim);
                if (user == null)
                {
                    return NotFound(new { message = "Kullanıcı bulunamadı" });
                }

                var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Şifre değiştirme başarısız: {Errors}", errors);
                    return BadRequest(new { message = errors });
                }

                _logger.LogInformation("Şifre başarıyla değiştirildi: {Email}", user.Email);
                return Ok(new { message = "Şifre başarıyla değiştirildi" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Şifre değiştirme işlemi sırasında hata oluştu");
                return StatusCode(500, new { message = "Sunucu hatası oluştu" });
            }
        }
    }

}
