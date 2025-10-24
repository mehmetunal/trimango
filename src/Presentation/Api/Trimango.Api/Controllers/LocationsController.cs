using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Maggsoft.Core.Base;
using Trimango.Mssql.Services.Interfaces;
using Trimango.Dto.Mssql.Location;

namespace Trimango.Api.Controllers
{
    /// <summary>
    /// Konum yönetimi API endpoint'leri
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        /// Tüm konumları listeler
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetLocations()
        {
            var locations = await _locationService.GetAllLocationsAsync();
            return Ok(locations);
        }

        /// <summary>
        /// ID'ye göre konum detayını getirir
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLocationById(Guid id)
        {
            var location = await _locationService.GetLocationByIdAsync(id);
            if (location == null)
                return NotFound();

            return Ok(location);
        }

        /// <summary>
        /// Şehre göre konumları listeler
        /// </summary>
        [HttpGet("city/{city}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLocationsByCity(string city)
        {
            var locations = await _locationService.GetLocationsByCityAsync(city);
            return Ok(locations);
        }

        /// <summary>
        /// Yeni konum oluşturur
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateLocation(CreateLocationDto dto)
        {
            var result = await _locationService.CreateLocationAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetLocationById), new { id = result.Data.Id }, result.Data);
        }

        /// <summary>
        /// Konum bilgilerini günceller
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLocation(Guid id, UpdateLocationDto dto)
        {
            var result = await _locationService.UpdateLocationAsync(id, dto);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result.Data);
        }

        /// <summary>
        /// Konumu siler (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLocation(Guid id)
        {
            var result = await _locationService.DeleteLocationAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result.Data);
        }
    }
}
