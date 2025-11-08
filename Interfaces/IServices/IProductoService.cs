using catalogo.Dtos.Producto;

namespace catalogo.Interfaces.IServices
{
    public interface IProductoService
    {
        Task<CrearProductoDto> CreateAsync(CrearProductoDto productoDto);
        Task<ActualizarProductoDto?> UpdateAsync(ActualizarProductoDto productoDto);
    }
}
