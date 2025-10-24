using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Maggsoft.Core.Base;
using Trimango.Mssql.Services.Interfaces;
using Trimango.Dto.Mssql.Supplier;

namespace Trimango.Api.Controllers.v1
{
    /// <summary>
    /// Tedarikçi yönetimi API endpoint'leri
    /// </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        /// <summary>
        /// Tüm tedarikçileri listeler
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(suppliers);
        }

        /// <summary>
        /// ID'ye göre tedarikçi detayını getirir
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetSupplierById(Guid id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }

        /// <summary>
        /// Yeni tedarikçi oluşturur
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSupplier(CreateSupplierDto dto)
        {
            var result = await _supplierService.CreateSupplierAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetSupplierById), new { id = result.Data.Id }, result.Data);
        }

        /// <summary>
        /// Tedarikçi bilgilerini günceller
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSupplier(Guid id, UpdateSupplierDto dto)
        {
            var result = await _supplierService.UpdateSupplierAsync(id, dto);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result.Data);
        }

        /// <summary>
        /// Tedarikçiyi siler (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSupplier(Guid id)
        {
            var result = await _supplierService.DeleteSupplierAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result.Data);
        }
    }
}