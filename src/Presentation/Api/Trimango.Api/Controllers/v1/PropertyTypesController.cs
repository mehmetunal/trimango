using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Maggsoft.Core.Base;
using Trimango.Mssql.Services.Interfaces;
using Trimango.Dto.Mssql.PropertyType;

namespace Trimango.Api.Controllers.v1
{
    /// <summary>
    /// Konaklama türü yönetimi API endpoint'leri
    /// </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class PropertyTypesController : ControllerBase
    {
        private readonly IPropertyTypeService _propertyTypeService;

        public PropertyTypesController(IPropertyTypeService propertyTypeService)
        {
            _propertyTypeService = propertyTypeService;
        }

        /// <summary>
        /// Tüm konaklama türlerini listeler
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPropertyTypes()
        {
            var propertyTypes = await _propertyTypeService.GetAllPropertyTypesAsync();
            return Ok(propertyTypes);
        }

        /// <summary>
        /// ID'ye göre konaklama türü detayını getirir
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPropertyTypeById(Guid id)
        {
            var propertyType = await _propertyTypeService.GetPropertyTypeByIdAsync(id);
            if (propertyType == null)
                return NotFound();

            return Ok(propertyType);
        }

        /// <summary>
        /// Yeni konaklama türü oluşturur
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePropertyType(CreatePropertyTypeDto dto)
        {
            var result = await _propertyTypeService.CreatePropertyTypeAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetPropertyTypeById), new { id = result.Data.Id }, result.Data);
        }

        /// <summary>
        /// Konaklama türü bilgilerini günceller
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePropertyType(Guid id, UpdatePropertyTypeDto dto)
        {
            var result = await _propertyTypeService.UpdatePropertyTypeAsync(id, dto);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result.Data);
        }

        /// <summary>
        /// Konaklama türünü siler (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePropertyType(Guid id)
        {
            var result = await _propertyTypeService.DeletePropertyTypeAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result.Data);
        }
    }
}
