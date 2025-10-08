using catalogo.Dtos.Atributo;

namespace catalogo.Interfaces.IServices
{
    public interface IAtributoValorService
    {
        Task<IEnumerable<AtributoValorDetalleDto>> ObtenerTodosAsync();
        Task<IEnumerable<AtributoValorDetalleDto>?> ObtenerPorAtributoIdAsync(int atributoId);
        Task<AtributoValorDetalleDto?> ObtenerPorIdAsync(int id);
        Task<AtributoValorDetalleDto?> CrearAsync(int atributoId, CrearAtributoValorDto crearAtributoValorDto);
        Task<AtributoValorDetalleDto?> ActualizarAsync(int id, ActualizarAtributoValorDto actualizarAtributoValorDto);
        Task<bool> EliminarAsync(int id);
    }
}
