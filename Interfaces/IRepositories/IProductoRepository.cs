using catalogo.Dtos.Producto;
using catalogo.Models;

namespace catalogo.Interfaces.IRepositories
{
    public interface IProductoRepository
    {
        Task<Producto?> GetByIdAsync(int id);
        Task<List<Producto>> GetAllAsync();
        Task<Producto> CreateAsync(Producto producto);
        Task<bool> DeleteAsync(int id);
        Task<List<ProductoListadoDto>> GetAllListadoAsync();
        Task SaveChangesAsync();
    }
}
