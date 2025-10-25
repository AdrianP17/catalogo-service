using catalogo.Dtos.Variante;

namespace catalogo.Interfaces.IServices
{
    public interface IVarianteService
    {
        Task<CrearVarianteDto?> CreateAsync(int idProducto, CrearVarianteDto varianteDto);
        Task<List<VarianteDto>?> GetByProductIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<ActualizarVarianteDto?> UpdateAsync(int idProducto, int idVariante, ActualizarVarianteDto varianteDto);
    }
}
