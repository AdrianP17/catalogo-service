using catalogo.Models;

namespace catalogo.Interfaces.IRepositories
{
    public interface IVarianteRepository
    {
        Task<bool> SkuExistsAsync(string sku);
        Task<bool> CombinationExistsAsync(int idProducto, List<int> idsAtributosValores);
        Task AddAsync(Variante variante);
        Task<int> SaveChangesAsync();
    }
}
