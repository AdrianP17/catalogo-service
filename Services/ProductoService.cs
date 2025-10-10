using catalogo.Dtos.Producto;
using catalogo.Interfaces.IRepositories;
using catalogo.Interfaces.IServices;
using catalogo.Models;

namespace catalogo.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repo;
        private readonly IAlmacenadorArchivos _almacenadorArchivos;
        private readonly IAtributoValorRepository _atributoValorRepository;
        private readonly string _containerName;

        public ProductoService(IProductoRepository repo, IAlmacenadorArchivos almacenadorArchivos, IAtributoValorRepository atributoValorRepository)
        {
            _repo = repo;
            _almacenadorArchivos = almacenadorArchivos;
            _containerName = "data";
            _atributoValorRepository = atributoValorRepository;
        }

        public async Task<CrearProductoDto> CreateAsync(CrearProductoDto productoDto)
        {
            var idsCategorias = await _atributoValorRepository.GetAtributosValoresByIdsAsync(productoDto.IdsCategorias);
            var producto = new Producto
            {
                Nombre = productoDto.Nombre,
                Descripcion = productoDto.Descripcion,
                ProductoAtributos = idsCategorias.Select(c => new ProductoAtributo { AtributoValorId = c.Id }).ToList()
            };

            // Procesar y subir cada imagen
            var productoImagenes = new List<ProductoImagen>();
            int index = 0;

            foreach (var imagenArchivo in productoDto.Imagenes)
            {
                var url = await _almacenadorArchivos.SubirArchivoAsync(imagenArchivo, _containerName);

                productoImagenes.Add(new ProductoImagen
                {
                    Imagen = url,
                    Principal = index == 0
                });
                
                index++;
            }

            producto.ProductoImagenes = productoImagenes;

            await _repo.CreateAsync(producto);

            return productoDto;
        }
    }
}
