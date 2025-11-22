using catalogo.Dtos.Producto;

namespace catalogo.Interfaces.IServices
{
    public interface IProductoCargaMasivaService
    {
        Task<IEnumerable<ProductoDetalleDto>> CargarProductosDesdeCsvAsync(IFormFile archivoCsv, ICollection<IFormFile> imagenes);
    }
}
