using catalogo.Data;
using catalogo.Dtos;
using catalogo.Dtos.Producto;
using catalogo.Helpers;
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
            return await _context.Producto.Include(p => p.ProductoImagenes).Include(p => p.ProductoAtributos).ThenInclude(pa => pa.AtributoValor).Include(p => p.Variantes).ToListAsync();
        }

        public async Task<Producto?> GetByIdAsync(int id)
        {
            var producto = await _context.Producto.Include(p => p.ProductoImagenes).Include(p => p.ProductoAtributos).Include(p => p.Variantes).ThenInclude(v => v.VarianteAtributos).Include(p => p.Variantes).ThenInclude(v => v.VarianteImagenes).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null) return null;
            return producto;
        }

        public async Task<PaginationResponse<ProductoListadoDto>> GetAllListadoAsync(QueryObject query)
        {
            var productosQuery = _context.Producto
                .Include(p => p.ProductoAtributos).ThenInclude(pa => pa.AtributoValor).ThenInclude(av => av.Atributo)
                .Include(p => p.Variantes).ThenInclude(v => v.VarianteAtributos).ThenInclude(va => va.AtributoValor).ThenInclude(av => av.Atributo)
                .Include(p => p.ProductoImagenes)
                .AsQueryable();

            // Por categoría
            if (!string.IsNullOrEmpty(query.Categoria))
            {
                productosQuery = productosQuery.Where(p =>
                    p.ProductoAtributos.Any(pa =>
                        pa.AtributoValor.Atributo.Nombre.ToLower() == "categoría" &&
                        pa.AtributoValor.Valor.ToLower() == query.Categoria.ToLower()
                    )
                );
            }

            // Por color
            if (!string.IsNullOrEmpty(query.Color))
            {
                productosQuery = productosQuery.Where(p =>
                    p.Variantes.Any(v =>
                        v.VarianteAtributos.Any(va =>
                            va.AtributoValor.Atributo.Nombre.ToLower() == "color" &&
                            va.AtributoValor.Valor.ToLower() == query.Color.ToLower()
                        )
                    )
                );
            }

            // Por talla
            if (!string.IsNullOrEmpty(query.Talla))
            {
                productosQuery = productosQuery.Where(p =>
                    p.Variantes.Any(v =>
                        v.VarianteAtributos.Any(va =>
                            va.AtributoValor.Atributo.Nombre.ToLower() == "talla" &&
                            va.AtributoValor.Valor.ToLower() == query.Talla.ToLower()
                        )
                    )
                );
            }

            // Por precio
            if (query.PrecioMin.HasValue)
            {
              productosQuery = productosQuery.Where(p =>
                  (!p.Variantes.Any() && query.PrecioMin.Value <= 0) || 
                  p.Variantes.Any(v => v.Precio >= query.PrecioMin.Value)
                  );
            }

            if (query.PrecioMax.HasValue)
            {
              productosQuery = productosQuery.Where(p =>
                  (!p.Variantes.Any() && query.PrecioMax.Value >= 0) || 
                  p.Variantes.Any(v => v.Precio <= query.PrecioMax.Value)
                  );
            }

            int total = await productosQuery.CountAsync();

            int skip = (query.PageNumber - 1) * query.PageSize;

            var productosFiltered = await productosQuery.Select(p=>new ProductoListadoDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Variantes.Min(v => (decimal?)v.Precio) ?? 0m,
                Imagen = p.ProductoImagenes
                    .Where(img => img.Principal == true)
                    .Select(img => img.Imagen)
                    .FirstOrDefault() ?? string.Empty,
                TienePromocion = p.IdPromocion != null
            }).Skip(skip).Take(query.PageSize).ToListAsync();

            return new PaginationResponse<ProductoListadoDto>
            {
                Data = productosFiltered,
                Total = total,
                CurrentPage = query.PageNumber,
                ItemsPerPage = query.PageSize,
            };
        }


        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
