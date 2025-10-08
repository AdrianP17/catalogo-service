using catalogo.Models;

namespace catalogo.Interfaces.IRepositories
{
    public interface IAtributoValorRepository
    {
        Task<IEnumerable<AtributoValor>> ObtenerTodosAsync();
        Task<IEnumerable<AtributoValor>> ObtenerPorAtributoIdAsync(int atributoId);
        Task<AtributoValor?> ObtenerPorIdAsync(int id);
        Task<AtributoValor> CrearAsync(AtributoValor atributoValor);
        Task<AtributoValor?> ActualizarAsync(int id, AtributoValor atributoValor);
        Task<IEnumerable<AtributoValor>> GetAtributosValoresByIdsAsync(ICollection<int> ids);
        Task<bool> EliminarAsync(int id);
    }
}
