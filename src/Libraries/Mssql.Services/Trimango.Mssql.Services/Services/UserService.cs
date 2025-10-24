using AutoMapper;
using Maggsoft.Core.Base;
using Maggsoft.Core.IoC;
using Maggsoft.Core.Model;
using Maggsoft.Mssql.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Trimango.Data.Mssql.Entities;
using Trimango.Dto.Mssql.User;
using Trimango.Mssql.Services.Interfaces;

namespace Trimango.Mssql.Services.Services
{
    /// <summary>
    /// Kullanıcı servis implementasyonu
    /// </summary>
    public class UserService : CustomBaseService, IService, IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Result<List<UserListDto>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userManager.Users
                    .Where(u => u.IsActive && !u.IsDeleted)
                    .OrderBy(u => u.FirstName)
                    .ThenBy(u => u.LastName)
                    .ToListAsync();

                var userDtos = _mapper.Map<List<UserListDto>>(users);
                return Success(userDtos);
            }
            catch (Exception ex)
            {
                return OperationFailed<List<UserListDto>>("Error.UserRetrievalException", new object[] { ex.Message });
            }
        }

        public async Task<Result<List<UserListDto>>> GetUsersAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            try
            {
                var query = _userManager.Users
                    .Where(u => u.IsActive && !u.IsDeleted);

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(u =>
                        u.FirstName.Contains(searchTerm) ||
                        u.LastName.Contains(searchTerm) ||
                        u.Email.Contains(searchTerm) ||
                        (u.PhoneNumber != null && u.PhoneNumber.Contains(searchTerm)));
                }

                var users = await query
                    .OrderBy(u => u.FirstName)
                    .ThenBy(u => u.LastName)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var userDtos = _mapper.Map<List<UserListDto>>(users);
                return Success(userDtos);
            }
            catch (Exception ex)
            {
                return OperationFailed<List<UserListDto>>("Error.UserRetrievalException", new object[] { ex.Message });
            }
        }

        public async Task<Result<UserDto?>> GetUserByIdAsync(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null || !user.IsActive || user.IsDeleted)
                {
                    return NotFound<UserDto>("Error.UserNotFound");
                }

                var userDto = _mapper.Map<UserDto>(user);
                return Success(userDto);
            }
            catch (Exception ex)
            {
                return OperationFailed<UserDto?>("Error.UserRetrievalException", new object[] { ex.Message });
            }
        }

        public async Task<Result<UserDto?>> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null || !user.IsActive || user.IsDeleted)
                {
                    return NotFound<UserDto>("Error.UserNotFound");
                }

                var userDto = _mapper.Map<UserDto>(user);
                return Success(userDto);
            }
            catch (Exception ex)
            {
                return OperationFailed<UserDto?>("Error.UserRetrievalException", new object[] { ex.Message });
            }
        }

        public async Task<Result<UserDto>> CreateUserAsync(CreateUserDto dto)
        {
            try
            {
                // Email kontrolü
                var existingUser = await _userManager.FindByEmailAsync(dto.Email);
                if (existingUser != null)
                {
                    return Conflict<UserDto>("Error.UserAlreadyExists");
                }

                // Yeni kullanıcı oluştur
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = dto.Email,
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    PhoneNumber = dto.PhoneNumber,
                    IsSupplier = dto.IsSupplier,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false
                };

                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return ValidationFailed<UserDto>("Error.UserCreationFailed", new object[] { errors });
                }

                // Kullanıcı rolü ata
                await _userManager.AddToRoleAsync(user, "User");

                var userDto = _mapper.Map<UserDto>(user);
                return Success(userDto);
            }
            catch (Exception ex)
            {
                return OperationFailed<UserDto>("Error.UserCreationException", new object[] { ex.Message });
            }
        }

        public async Task<Result<UserDto>> UpdateUserAsync(Guid id, UpdateUserDto dto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null || !user.IsActive || user.IsDeleted)
                {
                    return NotFound<UserDto>("Error.UserNotFound");
                }

                // Email kontrolü (kendi email'i değilse)
                if (user.Email != dto.Email)
                {
                    var existingUser = await _userManager.FindByEmailAsync(dto.Email);
                    if (existingUser != null && existingUser.Id != id)
                    {
                        return Conflict<UserDto>("Error.UserAlreadyExists");
                    }
                }

                // Kullanıcı bilgilerini güncelle
                user.Email = dto.Email;
                user.UserName = dto.Email;
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.PhoneNumber = dto.PhoneNumber;
                user.ProfilePictureUrl = dto.ProfilePictureUrl;
                user.IsSupplier = dto.IsSupplier;
                user.UpdatedDate = DateTime.UtcNow;
                user.UpdatedIP = RemoteIp;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return ValidationFailed<UserDto>("Error.UserUpdateFailed", new object[] { errors });
                }

                var userDto = _mapper.Map<UserDto>(user);
                return Success(userDto);
            }
            catch (Exception ex)
            {
                return OperationFailed<UserDto>("Error.UserUpdateException", new object[] { ex.Message });
            }
        }

        public async Task<Result<bool>> ChangePasswordAsync(Guid id, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null || !user.IsActive || user.IsDeleted)
                {
                    return NotFound<bool>("Error.UserNotFound");
                }

                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return ValidationFailed<bool>("Error.PasswordChangeFailed", new object[] { errors });
                }

                return Success(true);
            }
            catch (Exception ex)
            {
                return OperationFailed<bool>("Error.PasswordChangeException", new object[] { ex.Message });
            }
        }

        public async Task<Result<bool>> DeleteUserAsync(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null || !user.IsActive || user.IsDeleted)
                {
                    return NotFound<bool>("Error.UserNotFound");
                }

                // Soft delete
                user.SoftDelete();
                user.UpdatedDate = DateTime.UtcNow;
                user.UpdatedIP = RemoteIp;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return ValidationFailed<bool>("Error.UserDeleteFailed", [errors]);
                }

                return Success(true);
            }
            catch (Exception ex)
            {
                return OperationFailed<bool>("Error.UserDeleteException", [ex.Message]);
            }
        }

        public async Task<Result<bool>> RestoreUserAsync(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return NotFound<bool>("Error.UserNotFound");
                }

                // Restore
                user.Restore();
                user.UpdatedDate = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return ValidationFailed<bool>("Error.UserRestoreFailed", new object[] { errors });
                }

                return Success(true);
            }
            catch (Exception ex)
            {
                return OperationFailed<bool>("Error.UserRestoreException", new object[] { ex.Message });
            }
        }

        public async Task<Result<bool>> SendEmailConfirmationAsync(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null || !user.IsActive || user.IsDeleted)
                {
                    return NotFound<bool>("Error.UserNotFound");
                }

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // TODO: Email gönderme servisi entegre edilecek

                return Success(true);
            }
            catch (Exception ex)
            {
                return OperationFailed<bool>("Error.EmailConfirmationException", new object[] { ex.Message });
            }
        }

        public async Task<Result<bool>> ConfirmEmailAsync(Guid userId, string token)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null || !user.IsActive || user.IsDeleted)
                {
                    return NotFound<bool>("Error.UserNotFound");
                }

                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return ValidationFailed<bool>("Error.EmailConfirmationFailed", new object[] { errors });
                }

                return Success(true);
            }
            catch (Exception ex)
            {
                return OperationFailed<bool>("Error.EmailConfirmationException", new object[] { ex.Message });
            }
        }

        public async Task<Result<bool>> SendPasswordResetAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null || !user.IsActive || user.IsDeleted)
                {
                    return NotFound<bool>("Error.UserNotFound");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                // TODO: Email gönderme servisi entegre edilecek

                return Success(true);
            }
            catch (Exception ex)
            {
                return OperationFailed<bool>("Error.PasswordResetException", new object[] { ex.Message });
            }
        }

        public async Task<Result<bool>> ResetPasswordAsync(string email, string token, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null || !user.IsActive || user.IsDeleted)
                {
                    return NotFound<bool>("Error.UserNotFound");
                }

                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return ValidationFailed<bool>("Error.PasswordResetFailed", new object[] { errors });
                }

                return Success(true);
            }
            catch (Exception ex)
            {
                return OperationFailed<bool>("Error.PasswordResetException", new object[] { ex.Message });
            }
        }
    }
}