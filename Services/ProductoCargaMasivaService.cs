using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using catalogo.Data;
using catalogo.Dtos.Producto;
using catalogo.Interfaces.IRepositories;
using catalogo.Interfaces.IServices;
using catalogo.Models;

namespace catalogo.Services
{
    public class ProductoCargaMasivaService : IProductoCargaMasivaService
    {
        private class ProductoCsvRecord
        {
            public string Nombre { get; set; } = string.Empty;
            public string Descripcion { get; set; } = string.Empty;
            public string ImageFileNames { get; set; } = string.Empty;
            public string Categoría { get; set; } = string.Empty;
            public string Género { get; set; } = string.Empty;
            public string Deporte { get; set; } = string.Empty;
            public string Tipo { get; set; } = string.Empty;
            public string Colección { get; set; } = string.Empty;
        }

        private readonly AppDBContext _context;
        private readonly IAlmacenadorArchivos _almacenadorArchivos;
        private readonly IProductoRepository _productoRepository;
        private readonly IAtributoRepository _atributoRepository;
        private const string CONTENEDOR_IMAGENES = "data";
        private List<string> ProductoCategorias = new List<string> { "Categoría", "Género", "Deporte", "Tipo", "Colección" };

        public ProductoCargaMasivaService(
            AppDBContext context,
            IAlmacenadorArchivos almacenadorArchivos,
            IProductoRepository productoRepository,
            IAtributoRepository atributoRepository)
        {
            _context = context;
            _almacenadorArchivos = almacenadorArchivos;
            _productoRepository = productoRepository;
            _atributoRepository = atributoRepository;
        }

        public async Task<IEnumerable<ProductoDetalleDto>> CargarProductosDesdeCsvAsync(IFormFile archivoCsv, ICollection<IFormFile> imagenes)
        {
            if (archivoCsv == null || archivoCsv.Length == 0)
                throw new ArgumentException("El archivo CSV es obligatorio.");

            var nuevosProductos = new List<Producto>();
            await using var transaction = await _context.Database.BeginTransactionAsync();
            var mapaAtributos = await _atributoRepository.LoadAllAtributosAsync();

            try
            {
                // 1. Subir todas las imágenes y crear un diccionario para búsqueda rápida (si se proporcionan)
                var mapaImagenesUrl = new Dictionary<string, string>();
                if (imagenes != null && imagenes.Any())
                {
                    foreach (var imagen in imagenes)
                    {
                        var nombreSinExtension = Path.GetFileNameWithoutExtension(imagen.FileName);
                        if (mapaImagenesUrl.ContainsKey(nombreSinExtension)) continue; // Evitar duplicados

                        var url = await _almacenadorArchivos.SubirArchivoConNombreAsync(imagen, CONTENEDOR_IMAGENES, imagen.FileName);
                        mapaImagenesUrl.Add(nombreSinExtension, url);
                    }
                }

                // 2. Leer y procesar el archivo CSV
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    HeaderValidated = null, // Ignorar validación por problemas de encoding en k6
                    MissingFieldFound = null
                };
                using var reader = new StreamReader(archivoCsv.OpenReadStream(), Encoding.UTF8);
                using var csv = new CsvReader(reader, config);
                var records = csv.GetRecords<ProductoCsvRecord>().ToList();

                foreach (var record in records)
                {
                    var producto = new Producto
                    {
                        Nombre = record.Nombre,
                        Descripcion = record.Descripcion,
                        ProductoImagenes = new List<ProductoImagen>()
                    };

                    foreach (var atributoNombre in ProductoCategorias)
                    {
                        var valorCsv = record.GetType().GetProperty(atributoNombre)?.GetValue(record) as string;
                        valorCsv = valorCsv?.Trim().ToLower();

                        if (!string.IsNullOrWhiteSpace(valorCsv))
                        {
                            if (mapaAtributos.TryGetValue(atributoNombre, out var valoresAtributo) && valoresAtributo.TryGetValue(valorCsv, out var valorId))
                            {
                                producto.ProductoAtributos.Add(new ProductoAtributo { AtributoValorId = valorId });
                            }
                            else
                            {
                                Console.WriteLine($"Error: El valor '{valorCsv}' no existe para el atributo '{atributoNombre}'.");
                            }
                        }

                    }
                    // 3. Asociar imágenes al producto
                    if (!string.IsNullOrWhiteSpace(record.ImageFileNames))
                    {
                        var nombresImagenes = record.ImageFileNames.Split(',').Select(n => n.Trim());
                        foreach (var nombreImagenCsv in nombresImagenes)
                        {
                            var nombreLimpio = Path.GetFileNameWithoutExtension(nombreImagenCsv);

                            if (mapaImagenesUrl.TryGetValue(nombreLimpio, out var urlImagen))
                            {
                                producto.ProductoImagenes.Add(new ProductoImagen { Imagen = urlImagen });
                            }
                            else
                            {
                                Console.WriteLine($"Advertencia: No se encontró la imagen '{nombreLimpio}' para el producto '{record.Nombre}'.");
                            }
                        }
                    }

                    nuevosProductos.Add(producto);
                }

                // 4. Guardar todos los productos en la base de datos
                await _context.Producto.AddRangeAsync(nuevosProductos);
                await _context.SaveChangesAsync();

                // 5. Si todo fue bien, confirmar la transacción
                await transaction.CommitAsync();

                // 6. Devolver los productos creados
                var idsCreados = nuevosProductos.Select(p => p.Id).ToList();
                return await _productoRepository.GetDetallesAsync(idsCreados);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw; // Re-lanzar la excepción para que el controlador la maneje
            }
        }
    }
}
