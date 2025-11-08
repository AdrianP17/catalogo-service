using catalogo.Data;
using catalogo.Interfaces.IRepositories;
using catalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace catalogo.Repository
{
    public class VarianteRepository : IVarianteRepository
    {
        private readonly AppDBContext _context;

        public VarianteRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Variante variante)
        {
            await _context.Variante.AddAsync(variante);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> SkuExistsAsync(string sku)
        {
            return await _context.Variante
                .AnyAsync(v => v.Sku == sku);
        }

        public async Task<bool> CombinationExistsAsync(int idProducto, List<int> idsAtributosValores)
        {
            int expectedCount = idsAtributosValores.Count;

            return await _context.Variante
                .Where(v => v.ProductoId == idProducto)
                .AnyAsync(v =>
                    v.VarianteAtributos.Count() == expectedCount &&
                    v.VarianteAtributos.All(va => idsAtributosValores.Contains(va.AtributoValorId))
                );
        }

        public Task<Variante?> GetByIdAsync(int id)
        {
            return _context.Variante.Include(v => v.VarianteImagenes).Include(v => v.VarianteAtributos).ThenInclude(va => va.AtributoValor).FirstOrDefaultAsync(v => v.Id == id);
        }
    }
}
