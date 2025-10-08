using catalogo.Dtos.Atributo;
using catalogo.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace catalogo.Controllers
{
    [ApiController]
    public class AtributoValoresController : ControllerBase
    {
        private readonly IAtributoValorService _atributoValorService;

        public AtributoValoresController(IAtributoValorService atributoValorService)
        {
            _atributoValorService = atributoValorService;
        }

        [HttpGet("api/atributovalores")]
        public async Task<ActionResult<IEnumerable<AtributoValorDetalleDto>>> ObtenerTodos()
        {
            var atributoValores = await _atributoValorService.ObtenerTodosAsync();
            return Ok(atributoValores);
        }

        [HttpGet("api/atributos/{atributoId}/valores")]
        public async Task<ActionResult<IEnumerable<AtributoValorDetalleDto>>> ObtenerPorAtributoId(int atributoId)
        {
            var atributoValores = await _atributoValorService.ObtenerPorAtributoIdAsync(atributoId);
            if (atributoValores == null) return NotFound();
            return Ok(atributoValores);
        }

        [HttpPost("api/atributos/{atributoId}/valores")]
        public async Task<ActionResult<AtributoValorDetalleDto>> Crear(int atributoId, CrearAtributoValorDto crearAtributoValorDto)
        {
            var atributoValor = await _atributoValorService.CrearAsync(atributoId, crearAtributoValorDto);
            if (atributoValor == null)
                return NotFound();

            return Ok(atributoValor);
        }

        [HttpGet("api/atributos/{atributoId}/valores/{valorId}")]
        public async Task<ActionResult<AtributoValorDetalleDto>> ObtenerPorId(int atributoId, int valorId)
        {
            var atributoValor = await _atributoValorService.ObtenerPorIdAsync(valorId);
            if (atributoValor == null || atributoValor.AtributoId != atributoId)
                return NotFound();

            return Ok(atributoValor);
        }

        // [HttpPut("api/atributos/{atributoId}/valores/{valorId}")]
        // public async Task<ActionResult<AtributoValorDetalleDto>> Actualizar(int atributoId, int valorId, ActualizarAtributoValorDto actualizarAtributoValorDto)
        // {
        //     // Asegurar que el atributoId en la ruta coincida con el del DTO
        //     actualizarAtributoValorDto.AtributoId = atributoId;
        //
        //     var atributoValor = await _atributoValorService.ActualizarAsync(valorId, actualizarAtributoValorDto);
        //     if (atributoValor == null)
        //         return NotFound();
        //
        //     return Ok(atributoValor);
        // }

        // [HttpDelete("api/atributos/{atributoId}/valores/{valorId}")]
        // public async Task<ActionResult> Eliminar(int atributoId, int valorId)
        // {
        //     // Verificar primero que el valor pertenezca al atributo
        //     var atributoValor = await _atributoValorService.ObtenerPorIdAsync(valorId);
        //     if (atributoValor == null || atributoValor.AtributoId != atributoId)
        //         return NotFound();
        //
        //     var resultado = await _atributoValorService.EliminarAsync(valorId);
        //     if (!resultado)
        //         return NotFound();
        //
        //     return NoContent();
        // }
    }
}
