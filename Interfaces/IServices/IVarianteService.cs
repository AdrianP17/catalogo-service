using catalogo.Dtos.Variante;

namespace catalogo.Interfaces.IServices
{
    public interface IVarianteService
    {
        Task<CrearVarianteDto?> CreateAsync(int idProducto, CrearVarianteDto varianteDto);
        Task<List<VarianteDto>?> GetByProductIdAsync(int id);
    }
}
