using catalogo.Dtos.Atributo;
using catalogo.Interfaces.IRepositories;
using catalogo.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace catalogo.Controllers
{
    [ApiController]
    [Route("api/atributos")]
    public class AtributosController : ControllerBase
    {
        private readonly IAtributoService _atributoService;
        private readonly IAtributoRepository _atributoRepository;
        public AtributosController(IAtributoService atributoService, IAtributoRepository atributoRepository)
        {
            _atributoService = atributoService;
            _atributoRepository = atributoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var atributos = await _atributoService.GetAllAsync();
            return Ok(atributos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var atributo = await _atributoService.GetByIdAsync(id);
            if (atributo == null) return NotFound();

            return Ok(atributo);
        }

        [HttpGet("valores")]
        public async Task<IActionResult> GetAllValoresAsync()
        {
            var valores = await _atributoRepository.LoadAllAtributosAsync();
            return Ok(valores);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CrearAtributoDto dto)
        {
            var resultado = await _atributoService.CreateAsync(dto);
            return Ok(resultado);
        }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] ActualizarAtributoDto atributo)
        // {
        //     var resultado = await _atributoService.UpdateAsync(id, atributo);
        //     if (resultado == null) return NotFound();
        //
        //     return NoContent();
        // }

        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        // {
        //     var resultado = await _atributoService.DeleteAsync(id);
        //     if (resultado) return NoContent();
        //     return NotFound();
        // }
    }
}
