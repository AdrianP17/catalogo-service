using Xunit;
using Moq;
using catalogo.Interfaces.IRepositories;
using catalogo.Services;
using catalogo.Dtos.Producto;
using catalogo.Models;
namespace catalogo.Test
{
    public class ProductoServiceTest
    {
        private readonly Mock<IProductoRepository> _mockRepo;
        private readonly Mock<IAlmacenadorArchivos> _mockAlmacenador;
        private readonly Mock<IAtributoValorRepository> _mockAtributoRepo;

        private readonly ProductoService _service;

        public ProductoServiceTest()
        {
            // 2. Inicializamos los mocks
            _mockRepo = new Mock<IProductoRepository>();
            _mockAlmacenador = new Mock<IAlmacenadorArchivos>();
            _mockAtributoRepo = new Mock<IAtributoValorRepository>();

            // 3. Inyectamos los mocks en el servicio REAL
            _service = new ProductoService(
                _mockRepo.Object,
                _mockAlmacenador.Object,
                _mockAtributoRepo.Object
            );
        }

        [Fact]
        public async Task CreateAsync_DebeSubirImagenes_Y_MarcarPrimeraComoPrincipal()
        {
            //arrange
            var imagenesMock = new List<IFormFile>
          {
            new Mock<IFormFile>().Object,
            new Mock<IFormFile>().Object
          };

            var productoDto = new CrearProductoDto
            {
                Nombre = "Polo deportivo",
                Descripcion = "Polo deportivo para adolescentes",
                IdsCategorias = new List<int> { 1 },
                Imagenes = imagenesMock
            };

            _mockAtributoRepo.Setup(repo => repo.GetAtributosValoresByIdsAsync(It.IsAny<List<int>>()))
                        .ReturnsAsync(new List<AtributoValor> { new AtributoValor { Id = 1 } });

            // Simulamos que Azure responde una URL falsa inmediatamente
            _mockAlmacenador.Setup(a => a.SubirArchivoAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ReturnsAsync("https://azure-fake.com/imagen.jpg");

            //act
            await _service.CreateAsync(productoDto);

            //assert
            _mockAlmacenador.Verify(x => x.SubirArchivoAsync(It.IsAny<IFormFile>(), "data"), Times.Exactly(2));

            _mockRepo.Verify(repo => repo.CreateAsync(It.Is<Producto>(p =>
                    p.ProductoImagenes.Count == 2 &&
                    p.ProductoImagenes.First().Principal == true &&
                    p.ProductoImagenes.Last().Principal == false)), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_DebeEliminarImagenesQueNoEstanEnElDto_Y_MantenerLasOtras()
        {
            // ARRANGE (Preparar el escenario)

            // 1. Datos de prueba: URLs
            var urlParaMantener = "https://azure.com/foto-vacaciones.jpg";
            var urlParaEliminar = "https://azure.com/foto-borrosa.jpg";

            // 2. Simulamos el producto que YA existe en la Base de Datos
            // Imagina que este producto tiene 2 fotos actualmente.
            var productoExistenteEnBd = new Producto
            {
                Id = 99,
                Nombre = "Cámara Vieja",
                ProductoImagenes = new List<ProductoImagen>
        {
            new ProductoImagen { Imagen = urlParaMantener }, // Esta se queda
            new ProductoImagen { Imagen = urlParaEliminar }  // Esta se va
        },
                ProductoAtributos = new List<ProductoAtributo>() // Inicializamos para que no falle el Clear()
            };

            // 3. El DTO que envía el usuario (El estado deseado)
            // NOTA: En 'ImagenesExistentesUrls' SOLO ponemos la que queremos mantener.
            var productoDto = new ActualizarProductoDto
            {
                Id = 99,
                Nombre = "Cámara Actualizada",
                ImagenesExistentesUrls = new List<string> { urlParaMantener }, // ¡Aquí omitimos la urlParaEliminar!
                NuevasImagenesArchivos = new List<IFormFile>(), // No agregamos nuevas por ahora para aislar la prueba
                IdsAtributosValores = new List<int>()
            };

            // 4. Configurar el Mock del Repositorio para devolver nuestro producto simulado
            _mockRepo.Setup(repo => repo.GetProductoEditableByIdAsync(99))
                .ReturnsAsync(productoExistenteEnBd);

            // ACT (Ejecutar)
            await _service.UpdateAsync(productoDto);

            // ASSERT (La hora de la verdad)

            // Check 1: ¿Llamó al almacenador para eliminar la foto borrosa?
            _mockAlmacenador.Verify(a => a.EliminarArchivoAsync(urlParaEliminar, "data"), Times.Once);

            // Check 2: ¿Aseguramos que NO eliminó la foto buena? (Crucial para evitar desastres)
            _mockAlmacenador.Verify(a => a.EliminarArchivoAsync(urlParaMantener, It.IsAny<string>()), Times.Never);

            // Check 3: ¿Se actualizó la lista en memoria del objeto producto?
            // La lista de imágenes del objeto debería tener ahora solo 1 elemento.
            Assert.Single(productoExistenteEnBd.ProductoImagenes);
            Assert.Equal(urlParaMantener, productoExistenteEnBd.ProductoImagenes.First().Imagen);

            // Check 4: ¿Se guardaron los cambios en la DB?
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_RetornaNull_SiProductoNoExiste()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetProductoEditableByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync((Producto)null); // Simulamos que no encuentra nada

            var dto = new ActualizarProductoDto { Id = 123 };

            // Act
            var resultado = await _service.UpdateAsync(dto);

            // Assert
            Assert.Null(resultado);
            // Aseguramos que NUNCA intentó guardar nada si no existía el producto
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
