using Maggsoft.Core.Base;
using Maggsoft.Core.IoC;
using Maggsoft.Core.Model.Pagination;
using Trimango.Dto.Mssql.User;

namespace Trimango.Mssql.Services.Interfaces
{
    /// <summary>
    /// Kullanıcı servis interface'i
    /// </summary>
    public interface IUserService : IService
    {
        /// <summary>
        /// Tüm kullanıcıları getirir
        /// </summary>
        Task<Result<List<UserListDto>>> GetAllUsersAsync();

        /// <summary>
        /// Sayfalı kullanıcı listesi getirir
        /// </summary>
        Task<Result<List<UserListDto>>> GetUsersAsync(int pageNumber, int pageSize, string? searchTerm = null);

        /// <summary>
        /// ID'ye göre kullanıcı getirir
        /// </summary>
        Task<Result<UserDto?>> GetUserByIdAsync(Guid id);

        /// <summary>
        /// Email'e göre kullanıcı getirir
        /// </summary>
        Task<Result<UserDto?>> GetUserByEmailAsync(string email);

        /// <summary>
        /// Yeni kullanıcı oluşturur
        /// </summary>
        Task<Result<UserDto>> CreateUserAsync(CreateUserDto dto);

        /// <summary>
        /// Kullanıcı günceller
        /// </summary>
        Task<Result<UserDto>> UpdateUserAsync(Guid id, UpdateUserDto dto);

        /// <summary>
        /// Kullanıcı şifresini değiştirir
        /// </summary>
        Task<Result<bool>> ChangePasswordAsync(Guid id, string currentPassword, string newPassword);

        /// <summary>
        /// Kullanıcıyı soft delete yapar
        /// </summary>
        Task<Result<bool>> DeleteUserAsync(Guid id);

        /// <summary>
        /// Kullanıcıyı geri yükler
        /// </summary>
        Task<Result<bool>> RestoreUserAsync(Guid id);

        /// <summary>
        /// Email doğrulama token'ı gönderir
        /// </summary>
        Task<Result<bool>> SendEmailConfirmationAsync(Guid userId);

        /// <summary>
        /// Email doğrulama token'ını doğrular
        /// </summary>
        Task<Result<bool>> ConfirmEmailAsync(Guid userId, string token);

        /// <summary>
        /// Şifre sıfırlama token'ı gönderir
        /// </summary>
        Task<Result<bool>> SendPasswordResetAsync(string email);

        /// <summary>
        /// Şifre sıfırlama token'ını doğrular ve yeni şifre belirler
        /// </summary>
        Task<Result<bool>> ResetPasswordAsync(string email, string token, string newPassword);
    }
}
