using catalogo.Data;
using catalogo.Dtos.Atributo;
using catalogo.Dtos.Variante;
using catalogo.Interfaces.IRepositories;
using catalogo.Interfaces.IServices;
using catalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace catalogo.Services
{
    public class VarianteService : IVarianteService
    {
        private readonly AppDBContext _context;
        private readonly IProductoRepository _productoRepository;
        private readonly IVarianteRepository _varianteRepository;
        private readonly IAtributoValorRepository _atributoRepository;
        private readonly IAlmacenadorArchivos _almacenadorArchivos;
        private readonly string _containerName = "data";

        public VarianteService(AppDBContext context, IProductoRepository productoRepository, IVarianteRepository varianteRepository, IAtributoValorRepository atributoRepository, IAlmacenadorArchivos almacenadorArchivos)
        {
            _context = context;
            _productoRepository = productoRepository;
            _varianteRepository = varianteRepository;
            _atributoRepository = atributoRepository;
            _almacenadorArchivos = almacenadorArchivos;
        }

        public async Task<CrearVarianteDto?> CreateAsync(int idProducto, CrearVarianteDto varianteDto)
        {

            if (varianteDto.IdsAtributosValores == null || varianteDto.IdsAtributosValores.Count == 0)
            {
                throw new Exception("La variante debe tener al menos un atributo.");
            }

            var producto = await _productoRepository.GetByIdAsync(idProducto);
            if (producto == null) return null;

            if (await _varianteRepository.SkuExistsAsync(varianteDto.Sku))
            {
                throw new Exception($"El SKU {varianteDto.Sku} ya existe");
            }

            if (varianteDto.Imagenes == null || varianteDto.Imagenes.Count == 0)
            {
                throw new Exception("La variante debe tener al menos una imagen.");
            }

            var atributosValores = await _atributoRepository.GetAtributosValoresByIdsAsync(varianteDto.IdsAtributosValores);

            if (!atributosValores.Any(av => av.Atributo.Nombre == "Color"))
            {
                throw new Exception("La variante debe incluir al menos un valor del atributo 'Color'.");
            }

            var varianteImagenes = new List<VarianteImagen>();
            foreach (var imagen in varianteDto.Imagenes)
            {
                var url = await _almacenadorArchivos.SubirArchivoAsync(imagen, _containerName);
                
                varianteImagenes.Add(new VarianteImagen
                {
                    Imagen = url
                });
            }

            var nuevaVariante = new Variante
                {
                    ProductoId = idProducto,
                    Sku = varianteDto.Sku,
                    Precio = varianteDto.Precio,
                    
                    // Creación de VarianteAtributo (relación limpia)
                    VarianteAtributos = varianteDto.IdsAtributosValores.Select(id => new VarianteAtributo
                    {
                        AtributoValorId = id
                    }).ToList(),
                    
                    VarianteImagenes = varianteImagenes
                };

            await _varianteRepository.AddAsync(nuevaVariante);
            await _varianteRepository.SaveChangesAsync();

            return varianteDto;
        }

        public async Task<List<VarianteDto>?> GetByProductIdAsync(int id)
        {
            var producto = await _context.Producto
                .Include(p => p.Variantes)
                  .ThenInclude(v => v.VarianteImagenes)
                .Include(p => p.Variantes)
                  .ThenInclude(v => v.VarianteAtributos)
                  .ThenInclude(va => va.AtributoValor)
              .FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null) return null;

            return producto.Variantes.Select(v => new VarianteDto
            {
                Id = v.Id,
                Precio = v.Precio,
                Imagenes = v.VarianteImagenes.Select(vi => vi.Imagen).ToList(),
                Atributos = v.VarianteAtributos
                .Select(va => new AtributoValorDto
                {
                    Id = va.AtributoValor.Id,
                    Valor = va.AtributoValor.Valor
                })
                .ToList()
            }).ToList();
        }
    }
}
