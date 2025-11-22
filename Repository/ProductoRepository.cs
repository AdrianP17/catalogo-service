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

            // TODO: Verificar que no haya ordenes asociadas a este producto

            _context.Producto.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Producto>> GetAllAsync()
        {
            return await _context.Producto.Include(p => p.ProductoImagenes).Include(p => p.ProductoAtributos).ThenInclude(pa => pa.AtributoValor).Include(p => p.Variantes).ThenInclude(v => v.VarianteAtributos).Include(p => p.Variantes).ThenInclude(v => v.VarianteImagenes).ToListAsync();
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
            if (query.Categoria != null && query.Categoria.Any())
            {
                var categorias = query.Categoria.Select(c => c.ToLower()).ToList();
                productosQuery = productosQuery.Where(p =>
                    p.ProductoAtributos.Any(pa =>
                        pa.AtributoValor.Atributo.Nombre.ToLower() == "categoría" &&
                        categorias.Contains(pa.AtributoValor.Valor.ToLower())
                    )
                );
            }

            // Por género
            if (query.Genero != null && query.Genero.Any())
            {
                var generos = query.Genero.Select(g => g.ToLower()).ToList();
                productosQuery = productosQuery.Where(p =>
                    p.ProductoAtributos.Any(pa =>
                        pa.AtributoValor.Atributo.Nombre.ToLower() == "género" &&
                        generos.Contains(pa.AtributoValor.Valor.ToLower())
                    )
                );
            }

            // Por deporte
            if (query.Deporte != null && query.Deporte.Any())
            {
                var deportes = query.Deporte.Select(d => d.ToLower()).ToList();
                productosQuery = productosQuery.Where(p =>
                    p.ProductoAtributos.Any(pa =>
                        pa.AtributoValor.Atributo.Nombre.ToLower() == "deporte" &&
                        deportes.Contains(pa.AtributoValor.Valor.ToLower())
                    )
                );
            }

            // Por tipo
            if (query.Tipo != null && query.Tipo.Any())
            {
                var tipos = query.Tipo.Select(t => t.ToLower()).ToList();
                productosQuery = productosQuery.Where(p =>
                    p.ProductoAtributos.Any(pa =>
                        pa.AtributoValor.Atributo.Nombre.ToLower() == "tipo" &&
                        tipos.Contains(pa.AtributoValor.Valor.ToLower())
                    )
                );
            }

            // Por colección
            if (query.Coleccion != null && query.Coleccion.Any())
            {
                var colecciones = query.Coleccion.Select(c => c.ToLower()).ToList();
                productosQuery = productosQuery.Where(p =>
                    p.ProductoAtributos.Any(pa =>
                        pa.AtributoValor.Atributo.Nombre.ToLower() == "colección" &&
                        colecciones.Contains(pa.AtributoValor.Valor.ToLower())
                    )
                );
            }

            // Por color
            if (query.Color != null && query.Color.Any())
            {
                var colores = query.Color.Select(c => c.ToLower()).ToList();
                productosQuery = productosQuery.Where(p =>
                    p.Variantes.Any(v =>
                        v.VarianteAtributos.Any(va =>
                            va.AtributoValor.Atributo.Nombre.ToLower() == "color" &&
                            colores.Contains(va.AtributoValor.Valor.ToLower())
                        )
                    )
                );
            }

            // Por talla
            if (query.Talla != null && query.Talla.Any())
            {
                var tallas = query.Talla.Select(t => t.ToLower()).ToList();
                productosQuery = productosQuery.Where(p =>
                    p.Variantes.Any(v =>
                        v.VarianteAtributos.Any(va =>
                            va.AtributoValor.Atributo.Nombre.ToLower() == "talla" &&
                            tallas.Contains(va.AtributoValor.Valor.ToLower())
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

            // Ordenar por precio
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Precio", StringComparison.OrdinalIgnoreCase))
                {
                    productosQuery = productosQuery.Where(p => p.Variantes != null && p.Variantes.Any());
                    productosQuery = query.IsDescending
                    ? productosQuery.OrderByDescending(p => p.Variantes.Min(v => (decimal?)v.Precio))
                    : productosQuery.OrderBy(p => p.Variantes.Min(v => (decimal?)v.Precio));
                }
            }

            int total = await productosQuery.CountAsync();

            int skip = (query.PageNumber - 1) * query.PageSize;

            var productosFiltered = await productosQuery.Select(p => new ProductoListadoDto
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

        public async Task<Producto?> GetProductoEditableByIdAsync(int id)
        {
            var producto = await _context.Producto.Include(p => p.ProductoImagenes).Include(p => p.ProductoAtributos).Include(p => p.Variantes).ThenInclude(v => v.VarianteAtributos).Include(p => p.Variantes).ThenInclude(v => v.VarianteImagenes).FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null) return null;
            return producto;
        }

        public Task<List<ProductoDetalleDto>> GetDetallesAsync(List<int> ids)
        {
            return _context.Producto
                .Where(p => ids.Contains(p.Id))
                .Select(p => new ProductoDetalleDto
                {
                    ProductoId = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Imagen = p.ProductoImagenes
                        .Where(img => img.Principal == true)
                        .Select(img => img.Imagen)
                        .FirstOrDefault()
                        ?? p.ProductoImagenes.Select(img => img.Imagen).FirstOrDefault()
                        ?? string.Empty
                })
                .ToListAsync();
        }
    }
}
