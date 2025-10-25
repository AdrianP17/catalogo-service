using catalogo.Dtos.Variante;
using catalogo.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace catalogo.Controllers
{
    [ApiController]
    [Route("api/variantes")]
    public class VariantesController : ControllerBase
    {
        private readonly IVarianteService _varianteService;
        public VariantesController(IVarianteService varianteService)
        {
            _varianteService = varianteService;
        }
        [HttpGet("productos/{id}/variantes")]
        public async Task<IActionResult> GetByProductId(int id)
        {
            var variantes = await _varianteService.GetByProductIdAsync(id);
            if (variantes == null) return NotFound("El producto no existe");

            return Ok(variantes);
        }

        [HttpPost("productos/{id}/variantes")]
        public async Task<IActionResult> Create([FromRoute] int id, [FromForm] CrearVarianteDto dto)
        {
            var resultado = await _varianteService.CreateAsync(id, dto);
            if (resultado == null) return NotFound("El producto no existe");

            return Ok(resultado);
        }

        [HttpPut("productos/{id}/variantes/{varianteId}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromRoute] int varianteId, [FromForm] ActualizarVarianteDto dto)
        {
            if (varianteId != dto.Id)
            {
                return BadRequest("El ID de la ruta no coincide con el ID de la variante.");
            }
            var resultado = await _varianteService.UpdateAsync(id, varianteId, dto);
            if (resultado == null) return NotFound("El producto o la variante no existe");

            return NoContent();
        }

        [HttpDelete("productos/{id}/variantes/{varianteId}")]
        public async Task<IActionResult> Delete([FromRoute] int id, [FromRoute] int varianteId)
        {
            var resultado = await _varianteService.DeleteAsync(varianteId);
            if (resultado == false) return NotFound("El producto o la variante no existe");

            return NoContent();
        }
    }
}
