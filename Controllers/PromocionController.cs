using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using catalogo.Data;
using catalogo.Dtos.Producto;
using catalogo.Dtos.Promocion;
using catalogo.Interfaces.IRepositories;
using catalogo.Mappers;

namespace Catalogo.Controllers
{
    [Route("/api/promociones")]
    [ApiController]
    public class PromocionController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IPromocionRepository _promocionRepository;
        public PromocionController(AppDBContext context, IPromocionRepository promocionRepository)
        {
            _promocionRepository = promocionRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var promociones = await _promocionRepository.GetAllAsync();
            var promocionesResult = promociones.Select(p => p.ToPromocionDto());

            return Ok(promocionesResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var promocion = await _promocionRepository.GetByIdAsync(id);

            if (promocion == null)
            {
                return NotFound();
            }

            return Ok(promocion.ToPromocionDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePromocionRequestDto insertDto)
        {
            var promocion = insertDto.ToPromocionFromCreateDto();

            await _promocionRepository.CreateAsync(promocion);

            return CreatedAtAction(nameof(GetById), new { id = promocion.Id }, promocion.ToPromocionDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePromocionRequestDto updateDto)
        {
            var promocion = await _promocionRepository.UpdateAsync(id, updateDto);

            if (promocion == null)
            {
                return NotFound();
            }

            return Ok(promocion.ToPromocionDto());
        }

        [HttpGet("{id}/productos")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductosPorPromocion([FromRoute] int id)
        {
            var promocion = await _promocionRepository.GetByIdAsync(id);
            if (promocion == null)
                return NotFound(new { mensaje = $"Promoción con id {id} no encontrada." });

            var productos = await _context.Producto
                .Where(p => p.PromocionId == id)
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    TienePromocion = p.PromocionId != null ? true : false,
                    IdPromocion = p.PromocionId
                })
                .ToListAsync();

            return Ok(productos);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var promocion = await _promocionRepository.DeleteAsync(id);

            if (promocion == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
