using Maggsoft.Core.Base;
using Maggsoft.Core.IoC;
using Maggsoft.Mssql.Repository;
using Trimango.Data.Mssql.Entities;
using Trimango.Mssql.Services.Interfaces;
using Trimango.Dto.Mssql.Location;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Trimango.Mssql.Services.Services
{
    /// <summary>
    /// Konum işlemleri için servis implementasyonu
    /// </summary>
    public class LocationService : CustomBaseService, IService, ILocationService
    {
        private readonly IMssqlRepository<Location> _locationRepository;
        private readonly IMapper _mapper;

        public LocationService(
            IMssqlRepository<Location> locationRepository,
            IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<List<LocationDto>> GetAllLocationsAsync()
        {
            var locations = await _locationRepository.Table
                .Where(l => l.IsActive && !l.IsDeleted)
                .OrderBy(l => l.City)
                .ThenBy(l => l.District)
                .ToListAsync();
            return _mapper.Map<List<LocationDto>>(locations);
        }

        public async Task<LocationDto?> GetLocationByIdAsync(Guid id)
        {
            var location = await _locationRepository.Table
                .FirstOrDefaultAsync(l => l.Id == id && l.IsActive && !l.IsDeleted);
            if (location == null) return null;
            
            return _mapper.Map<LocationDto>(location);
        }

        public async Task<List<LocationDto>> GetLocationsByCityAsync(string city)
        {
            var locations = await _locationRepository.Table
                .Where(l => l.City == city && l.IsActive && !l.IsDeleted)
                .OrderBy(l => l.District)
                .ToListAsync();
            return _mapper.Map<List<LocationDto>>(locations);
        }

        public async Task<Result<LocationDto>> CreateLocationAsync(CreateLocationDto dto)
        {
            try
            {
                var location = _mapper.Map<Location>(dto);
                location.CreatedDate = DateTime.UtcNow;
                location.CreatorUserId = CurrentUserId ?? Guid.Empty;
                location.IsActive = true;
                location.IsDeleted = false;

                await _locationRepository.AddAsync(location);
                await _locationRepository.SaveChangesAsync();

                var result = _mapper.Map<LocationDto>(location);
                return Success(result);
            }
            catch (Exception ex)
            {
                return OperationFailed<LocationDto>("Error.LocationCreationException", new object[] { ex.Message });
            }
        }

        public async Task<Result<LocationDto>> UpdateLocationAsync(Guid id, UpdateLocationDto dto)
        {
            try
            {
                var location = await _locationRepository.Table
                    .FirstOrDefaultAsync(l => l.Id == id && l.IsActive && !l.IsDeleted);
                if (location == null)
                    return LocationNotFound<LocationDto>();

                _mapper.Map(dto, location);
                location.UpdatedDate = DateTime.UtcNow;
                location.UpdatedByUserId = CurrentUserId;
                location.UpdatedIP = RemoteIp;

                await _locationRepository.UpdateAsync(location);
                await _locationRepository.SaveChangesAsync();

                var result = _mapper.Map<LocationDto>(location);
                return Success(result);
            }
            catch (Exception ex)
            {
                return OperationFailed<LocationDto>("Error.LocationUpdateException", new object[] { ex.Message });
            }
        }

        public async Task<Result<bool>> DeleteLocationAsync(Guid id)
        {
            try
            {
                var location = await _locationRepository.Table
                    .FirstOrDefaultAsync(l => l.Id == id && l.IsActive && !l.IsDeleted);
                if (location == null)
                    return LocationNotFound<bool>();

                location.SoftDelete();
                location.UpdatedDate = DateTime.UtcNow;
                location.UpdatedByUserId = CurrentUserId;
                location.UpdatedIP = RemoteIp;

                await _locationRepository.UpdateAsync(location);
                await _locationRepository.SaveChangesAsync();

                return Success(true);
            }
            catch (Exception ex)
            {
                return OperationFailed<bool>("Error.LocationDeleteException", new object[] { ex.Message });
            }
        }
    }
}
