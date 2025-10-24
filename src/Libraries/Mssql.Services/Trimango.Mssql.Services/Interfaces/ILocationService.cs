using Maggsoft.Core.Base;
using Maggsoft.Core.IoC;
using Trimango.Dto.Mssql.Location;

namespace Trimango.Mssql.Services.Interfaces
{
    /// <summary>
    /// Konum işlemleri için servis arayüzü
    /// </summary>
    public interface ILocationService : IService
    {
        Task<List<LocationDto>> GetAllLocationsAsync();
        Task<LocationDto?> GetLocationByIdAsync(Guid id);
        Task<List<LocationDto>> GetLocationsByCityAsync(string city);
        Task<Result<LocationDto>> CreateLocationAsync(CreateLocationDto dto);
        Task<Result<LocationDto>> UpdateLocationAsync(Guid id, UpdateLocationDto dto);
        Task<Result<bool>> DeleteLocationAsync(Guid id);
    }
}
