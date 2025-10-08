using catalogo.Dtos.Producto;
using catalogo.Interfaces.IRepositories;
using catalogo.Interfaces.IServices;
using catalogo.Models;

namespace catalogo.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repo;

        public ProductoService(IProductoRepository repo)
        {
            _repo = repo;
        }

        public async Task<CrearProductoDto> CreateAsync(CrearProductoDto productoDto)
        {
            var producto = new Producto
            {
                Nombre = productoDto.Nombre,
                Descripcion = productoDto.Descripcion,
                ProductoImagenes = productoDto.ImagenesBase64.Select((img, index) => new ProductoImagen
                {
                    Imagen = $"example://imagen-{Guid.NewGuid()}",
                    Principal = index == 0
                }).ToList()
            };

            await _repo.CreateAsync(producto);

            return (productoDto);
        }
    }
}
