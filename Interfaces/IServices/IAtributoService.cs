using catalogo.Dtos.Atributo;
using catalogo.Models;

namespace catalogo.Interfaces.IServices
{
    public interface IAtributoService
    {
        Task<Atributo?> GetByIdAsync(int id);
        Task<List<Atributo>> GetAllAsync();
        Task<Atributo> CreateAsync(CrearAtributoDto atributo);
        Task<Atributo?> UpdateAsync(int id, ActualizarAtributoDto atributo);
        Task<bool> DeleteAsync(int id);
    }
}
