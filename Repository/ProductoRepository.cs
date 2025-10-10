using catalogo.Data;
using catalogo.Dtos.Producto;
using catalogo.Interfaces.IRepositories;
using catalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace catalogo.Repository
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDBContext _context;
        public ProductoRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Producto> CreateAsync(Producto producto)
        {
            await _context.Producto.AddAsync(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var producto = await _context.Producto.FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null) return false;

            _context.Producto.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Producto>> GetAllAsync()
        {
            return await _context.Producto.Include(p => p.ProductoImagenes).Include(p => p.Variantes).ToListAsync();
        }

        public async Task<Producto?> GetByIdAsync(int id)
        {
            var producto = await _context.Producto.Include(p => p.ProductoImagenes).Include(p => p.Variantes).ThenInclude(v=> v.VarianteAtributos).Include(p => p.Variantes).ThenInclude(v => v.VarianteImagenes).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null) return null;
            return producto;
        }

        public async Task<List<ProductoListadoDto>> GetAllListadoAsync()
        {
            var productos = await _context.Producto.Select(p => new ProductoListadoDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Variantes.Min(v => (decimal?)v.Precio) ?? 0m,
                Imagen = p.ProductoImagenes.Where(img => img.Principal == true).Select(img => img.Imagen).FirstOrDefault() ?? string.Empty,
                TienePromocion = p.IdPromocion != null
            }).ToListAsync();

            return productos;
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
