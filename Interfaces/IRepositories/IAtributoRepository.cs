using catalogo.Models;

namespace catalogo.Interfaces.IRepositories
{
    public interface IAtributoRepository
    {
        Task<Atributo?> GetByIdAsync(int id);
        Task<List<Atributo>> GetAllAsync();
        Task<Atributo> CreateAsync(Atributo atributo);
        Task<bool> DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
