using Xunit;
using Moq;
using catalogo.Interfaces.IRepositories;
using catalogo.Services;
using catalogo.Dtos.Variante;
using catalogo.Models;
using catalogo.Data;
using Microsoft.EntityFrameworkCore;

namespace catalogo.Test
{
    // Source: https://docs.microsoft.com/en-us/ef/ef6/testing/mocking#mocking-with-moq
    public class AsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public AsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public T Current => _inner.Current;

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_inner.MoveNext());
        }

        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return new ValueTask();
        }
    }

    public class AsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly IEnumerable<T> _inner;

        public AsyncEnumerable(IEnumerable<T> inner)
        {
            _inner = inner;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new AsyncEnumerator<T>(_inner.GetEnumerator());
        }
    }


    public class VarianteServiceTest
    {
        private readonly Mock<IProductoRepository> _mockProductoRepo;
        private readonly Mock<IVarianteRepository> _mockVarianteRepo;
        private readonly Mock<IAtributoValorRepository> _mockAtributoValorRepo;
        private readonly Mock<IAlmacenadorArchivos> _mockAlmacenador;
        private readonly Mock<AppDBContext> _mockContext;
        private readonly VarianteService _service;

        public VarianteServiceTest()
        {
            _mockProductoRepo = new Mock<IProductoRepository>();
            _mockVarianteRepo = new Mock<IVarianteRepository>();
            _mockAtributoValorRepo = new Mock<IAtributoValorRepository>();
            _mockAlmacenador = new Mock<IAlmacenadorArchivos>();

            var options = new DbContextOptionsBuilder<AppDBContext>().Options;
            _mockContext = new Mock<AppDBContext>(options);

            _service = new VarianteService(
                _mockContext.Object,
                _mockProductoRepo.Object,
                _mockVarianteRepo.Object,
                _mockAtributoValorRepo.Object,
                _mockAlmacenador.Object
            );
        }

        private static Mock<DbSet<T>> GetMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();

            dbSet.As<IAsyncEnumerable<T>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new AsyncEnumerator<T>(queryable.GetEnumerator()));

            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(sourceList.Add);
            dbSet.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>(entity => sourceList.Remove(entity));

            return dbSet;
        }

        [Fact]
        public async Task CreateAsync_ThrowsException_WhenNoAttributesProvided()
        {
            // Arrange
            var varianteDto = new CrearVarianteDto { IdsAtributosValores = new List<int>() };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _service.CreateAsync(1, varianteDto));
            Assert.Equal("La variante debe tener al menos un atributo.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_ReturnsNull_WhenProductDoesNotExist()
        {
            // Arrange
            var varianteDto = new CrearVarianteDto { IdsAtributosValores = new List<int> { 1 } };
            _mockProductoRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Producto)null);

            // Act
            var result = await _service.CreateAsync(1, varianteDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ThrowsException_WhenSkuAlreadyExists()
        {
            // Arrange
            var varianteDto = new CrearVarianteDto
            {
                Sku = "SKU123",
                IdsAtributosValores = new List<int> { 1 }
            };
            _mockProductoRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Producto());
            _mockVarianteRepo.Setup(r => r.SkuExistsAsync("SKU123")).ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _service.CreateAsync(1, varianteDto));
            Assert.Equal("El SKU SKU123 ya existe", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_ThrowsException_WhenNoImagesProvided()
        {
            // Arrange
            var varianteDto = new CrearVarianteDto
            {
                Sku = "SKU123",
                IdsAtributosValores = new List<int> { 1 },
                Imagenes = new List<IFormFile>()
            };
            _mockProductoRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Producto());
            _mockVarianteRepo.Setup(r => r.SkuExistsAsync("SKU123")).ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _service.CreateAsync(1, varianteDto));
            Assert.Equal("La variante debe tener al menos una imagen.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_ThrowsException_WhenColorAttributeIsMissing()
        {
            // Arrange
            var imagenesMock = new List<IFormFile> { new Mock<IFormFile>().Object };
            var varianteDto = new CrearVarianteDto
            {
                Sku = "SKU123",
                IdsAtributosValores = new List<int> { 1 },
                Imagenes = imagenesMock
            };
            _mockProductoRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Producto());
            _mockVarianteRepo.Setup(r => r.SkuExistsAsync("SKU123")).ReturnsAsync(false);

            var atributosSinColor = new List<AtributoValor>
            {
                new AtributoValor { Id = 1, Valor = "XL", Atributo = new Atributo { Nombre = "Talla" } }
            };
            _mockAtributoValorRepo.Setup(r => r.GetAtributosValoresByIdsAsync(varianteDto.IdsAtributosValores))
                .ReturnsAsync(atributosSinColor);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _service.CreateAsync(1, varianteDto));
            Assert.Equal("La variante debe incluir al menos un valor del atributo 'Color'.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_SuccessfullyCreatesVariante()
        {
            // Arrange
            var mockImage = new Mock<IFormFile>();
            var varianteDto = new CrearVarianteDto
            {
                Sku = "SKU-SUCCESS",
                Precio = 99.99m,
                IdsAtributosValores = new List<int> { 1, 2 },
                Imagenes = new List<IFormFile> { mockImage.Object }
            };

            _mockProductoRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Producto { Id = 1 });
            _mockVarianteRepo.Setup(r => r.SkuExistsAsync(varianteDto.Sku)).ReturnsAsync(false);

            var atributosConColor = new List<AtributoValor>
            {
                new AtributoValor { Id = 1, Valor = "Rojo", Atributo = new Atributo { Nombre = "Color" } },
                new AtributoValor { Id = 2, Valor = "M", Atributo = new Atributo { Nombre = "Talla" } }
            };
            _mockAtributoValorRepo.Setup(r => r.GetAtributosValoresByIdsAsync(varianteDto.IdsAtributosValores))
                .ReturnsAsync(atributosConColor);

            _mockAlmacenador.Setup(a => a.SubirArchivoAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ReturnsAsync("https://fake-storage.com/image.jpg");

            // Act
            var result = await _service.CreateAsync(1, varianteDto);

            // Assert
            Assert.NotNull(result);
            _mockAlmacenador.Verify(a => a.SubirArchivoAsync(mockImage.Object, "data"), Times.Once);
            _mockVarianteRepo.Verify(r => r.AddAsync(It.Is<Variante>(v =>
                v.Sku == "SKU-SUCCESS" &&
                v.Precio == 99.99m &&
                v.VarianteAtributos.Count == 2 &&
                v.VarianteImagenes.Count == 1 &&
                v.VarianteImagenes.First().Imagen == "https://fake-storage.com/image.jpg"
            )), Times.Once);
            _mockVarianteRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        // [Fact]
        // public async Task DeleteAsync_ReturnsFalse_WhenVarianteDoesNotExist()
        // {
        //     // Arrange
        //     var mockSet = GetMockDbSet(new List<Variante>());
        //     _mockContext.Setup(c => c.Variante).Returns(mockSet.Object);

        //     // Act
        //     var result = await _service.DeleteAsync(999);

        //     // Assert
        //     Assert.False(result);
        // }

        // [Fact]
        // public async Task DeleteAsync_SuccessfullyDeletesVarianteAndImages()
        // {
        //     // Arrange
        //     var variantes = new List<Variante>
        //     {
        //         new Variante
        //         {
        //             Id = 1,
        //             VarianteImagenes = new List<VarianteImagen>
        //             {
        //                 new VarianteImagen { Imagen = "url1" },
        //                 new VarianteImagen { Imagen = "url2" }
        //             }
        //         }
        //     };
        //     var mockSet = GetMockDbSet(variantes);
        //     _mockContext.Setup(c => c.Variante).Returns(mockSet.Object);

        //     // Act
        //     var result = await _service.DeleteAsync(1);

        //     // Assert
        //     Assert.True(result);
        //     _mockAlmacenador.Verify(a => a.EliminarArchivoAsync("url1", "data"), Times.Once);
        //     _mockAlmacenador.Verify(a => a.EliminarArchivoAsync("url2", "data"), Times.Once);
        //     _mockContext.Verify(c => c.Variante.Remove(It.Is<Variante>(v => v.Id == 1)), Times.Once);
        //     _mockVarianteRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        // }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_IfVarianteDoesNotExist()
        {
            // Arrange
            var dto = new ActualizarVarianteDto { Id = 1 };
            _mockVarianteRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Variante)null);

            // Act
            var result = await _service.UpdateAsync(1, 1, dto);

            // Assert
            Assert.Null(result);
            _mockVarianteRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
