using catalogo.Dtos.Producto;
using catalogo.Helpers;
using catalogo.Interfaces.IRepositories;
using catalogo.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace catalogo.Controllers
{
    [ApiController]
    [Route("api/productos")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IProductoService _productoService;
        private readonly IProductoCargaMasivaService _productoCargaMasivaService;
        public ProductosController(
            IProductoRepository productoRepository,
            IProductoService productoService,
            IProductoCargaMasivaService productoCargaMasivaService)
        {
            _productoRepository = productoRepository;
            _productoService = productoService;
            _productoCargaMasivaService = productoCargaMasivaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productos = await _productoRepository.GetAllAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto == null) return NotFound();

            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CrearProductoDto dto)
        {
            if (dto.Imagenes == null || dto.Imagenes.Count == 0)
            {
                return BadRequest("El producto debe tener al menos una imagen.");
            }
            var resultado = await _productoService.CreateAsync(dto);
            return Ok(resultado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ActualizarProductoDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("El ID del producto no coincide con el ID enviado en el body.");
            }

            var productoActualizado = await _productoService.UpdateAsync(dto);
            if (productoActualizado == null) return NotFound();

            return Ok(productoActualizado);
        }

        [HttpGet("listado")]
        public async Task<IActionResult> GetAllListado([FromQuery] QueryObject query)
        {
            var productos = await _productoRepository.GetAllListadoAsync(query);
            return Ok(productos);
        }

        [HttpPost("detalles")]
        public async Task<IActionResult> GetDetalles([FromBody] List<int> ProductoIds)
        {
            var productos = await _productoRepository.GetDetallesAsync(ProductoIds);
            return Ok(productos);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _productoRepository.DeleteAsync(id);
            if (producto == false) return NotFound();

            return Ok();
        }

        [HttpPost("carga-masiva")]
        public async Task<IActionResult> CargaMasiva([FromForm] IFormFile archivoCsv, [FromForm] ICollection<IFormFile> imagenes)
        {
            try
            {
                var resultado = await _productoCargaMasivaService.CargarProductosDesdeCsvAsync(archivoCsv, imagenes);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno durante la carga masiva: {ex.Message}");
            }
        }
        
        [HttpGet("prueba")]
        public IActionResult Prueba(int id)
        {
            return Ok("Endpoint de prueba creado");
        }
    }
}
