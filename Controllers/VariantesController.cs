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
        public async Task<IActionResult> Create([FromRoute] int id, [FromBody] CrearVarianteDto dto)
        {
            var resultado = await _varianteService.CreateAsync(id, dto);
            if (resultado == null) return NotFound("El producto no existe");

            return Ok(resultado);
        }
    }
}
