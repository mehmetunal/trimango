using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Maggsoft.Core.Base;
using Trimango.Mssql.Services.Interfaces;
using Trimango.Dto.Mssql.Property;

namespace Trimango.Api.Controllers
{
    /// <summary>
    /// Konaklama yönetimi API endpoint'leri
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertiesController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        /// <summary>
        /// Tüm konaklamaları listeler
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProperties()
        {
            var properties = await _propertyService.GetAllPropertiesAsync();
            return Ok(properties);
        }

        /// <summary>
        /// ID'ye göre konaklama detayını getirir
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetPropertyById(Guid id)
        {
            var property = await _propertyService.GetPropertyByIdAsync(id);
            if (property == null)
                return NotFound();

            return Ok(property);
        }

        /// <summary>
        /// Tedarikçi ID'sine göre konaklamaları listeler
        /// </summary>
        [HttpGet("supplier/{supplierId}")]
        [Authorize]
        public async Task<IActionResult> GetPropertiesBySupplierId(Guid supplierId)
        {
            var properties = await _propertyService.GetPropertiesBySupplierIdAsync(supplierId);
            return Ok(properties);
        }

        /// <summary>
        /// Yeni konaklama oluşturur
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Supplier")]
        public async Task<IActionResult> CreateProperty(CreatePropertyDto dto)
        {
            var result = await _propertyService.CreatePropertyAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetPropertyById), new { id = result.Data.Id }, result.Data);
        }

        /// <summary>
        /// Konaklama bilgilerini günceller
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Supplier")]
        public async Task<IActionResult> UpdateProperty(Guid id, UpdatePropertyDto dto)
        {
            var result = await _propertyService.UpdatePropertyAsync(id, dto);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result.Data);
        }

        /// <summary>
        /// Konaklamayı siler (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Supplier")]
        public async Task<IActionResult> DeleteProperty(Guid id)
        {
            var result = await _propertyService.DeletePropertyAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result.Data);
        }
    }
}