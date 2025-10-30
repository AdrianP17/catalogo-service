using catalogo.Dtos.Producto;
using catalogo.Dtos.Variante;
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

        public async Task<ActualizarProductoDto?> UpdateAsync(ActualizarProductoDto productoDto)
        {
            var productoExistente = await _repo.GetProductoEditableByIdAsync(productoDto.Id);
            if (productoExistente == null) return null;

            productoExistente.Nombre = productoDto.Nombre;
            productoExistente.Descripcion = productoDto.Descripcion;

            var imagenesAEliminar = productoExistente.ProductoImagenes
                .Where(img => !productoDto.ImagenesExistentesUrls.Contains(img.Imagen))
                .ToList();

            foreach (var imagen in imagenesAEliminar)
            {
                await _almacenadorArchivos.EliminarArchivoAsync(imagen.Imagen, _containerName);
                productoExistente.ProductoImagenes.Remove(imagen);
            }

            foreach (var imagenArchivo in productoDto.NuevasImagenesArchivos)
            {
                var url = await _almacenadorArchivos.SubirArchivoAsync(imagenArchivo, _containerName);
                productoExistente.ProductoImagenes.Add(new ProductoImagen
                {
                    Imagen = url,
                    Principal = productoExistente.ProductoImagenes.Count == 0
                });
            }

            productoExistente.ProductoAtributos.Clear();
            foreach (var id in productoDto.IdsAtributosValores)
            {
                productoExistente.ProductoAtributos.Add(new ProductoAtributo { AtributoValorId = id });
            }

            await _repo.SaveChangesAsync();

            return productoDto;
        }

        private async Task CrearNuevaVarianteDesdeDto(Producto productoPadre, ActualizarVarianteDto dto)
        {
            var nuevasImagenes = new List<VarianteImagen>();
            foreach (var archivo in dto.NuevasImagenesArchivos)
            {
                var url = await _almacenadorArchivos.SubirArchivoAsync(archivo, _containerName);
                nuevasImagenes.Add(new VarianteImagen { Imagen = url });
            }

            var nuevaVariante = new Variante
            {
                Sku = dto.Sku,
                Precio = dto.Precio,
                ProductoId = productoPadre.Id,
                VarianteAtributos = dto.IdsAtributosValores.Select(id => new VarianteAtributo { AtributoValorId = id }).ToList(),
                VarianteImagenes = nuevasImagenes
            };
            productoPadre.Variantes.Add(nuevaVariante);
        }

        private async Task ActualizarVarianteExistente(Producto productoPadre, ActualizarVarianteDto dto)
        {
            var varianteExistente = productoPadre.Variantes
                .FirstOrDefault(v => v.Id == dto.Id);

            if (varianteExistente == null) return;

            varianteExistente.Precio = dto.Precio;
            varianteExistente.Sku = dto.Sku;

            varianteExistente.VarianteAtributos.Clear();
            foreach (var id in dto.IdsAtributosValores)
            {
                varianteExistente.VarianteAtributos.Add(new VarianteAtributo { AtributoValorId = id });
            }

            var imagenesAEliminar = varianteExistente.VarianteImagenes
                .Where(img => !dto.ImagenesExistentesUrls.Contains(img.Imagen))
                .ToList();

            foreach (var imagen in imagenesAEliminar)
            {
                await _almacenadorArchivos.EliminarArchivoAsync(imagen.Imagen, _containerName);
                varianteExistente.VarianteImagenes.Remove(imagen);
            }

            foreach (var archivo in dto.NuevasImagenesArchivos)
            {
                var url = await _almacenadorArchivos.SubirArchivoAsync(archivo, _containerName);
                varianteExistente.VarianteImagenes.Add(new VarianteImagen { Imagen = url });
            }
        }
    }
}
