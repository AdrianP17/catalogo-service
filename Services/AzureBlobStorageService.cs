using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

public class AzureBlobStorageService : IAlmacenadorArchivos
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public AzureBlobStorageService(IConfiguration configuration)
    {
        var connectionString = configuration["AzureStorage:ConnectionString"];
        _containerName = configuration["AzureStorage:ContainerName"] ?? "data";

        if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(_containerName))
        {
            throw new InvalidOperationException("La configuración de Azure Storage es inválida o incompleta.");
        }

        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task EliminarArchivoAsync(string urlCompleta, string contenedor)
    {
        if (string.IsNullOrEmpty(urlCompleta))
        {
            return;
        }

        try
        {
            // 1. OBTENER EL NOMBRE DEL ARCHIVO (GUID) DE LA URL COMPLETA
            // Ejemplo: http://tucuenta.blob.core.windows.net/data/50d3a5a7-1d2a-4c2f-b88a-3e8e7c1f8a8b.jpg
            var uri = new Uri(urlCompleta);

            // uri.Segments contendrá: ["/", "data/", "50d3a5a7-1d2a-4c2f-b88a-3e8e7c1f8a8b.jpg"]
            // El nombre del archivo es el último segmento
            var nombreArchivo = uri.Segments.Last();

            // 2. CONECTARSE AL CONTENEDOR
            var containerClient = _blobServiceClient.GetBlobContainerClient(contenedor);

            // 3. OBTENER EL CLIENTE DEL BLOB
            var blobClient = containerClient.GetBlobClient(nombreArchivo);

            // 4. ELIMINAR EL BLOB
            // Opción: DeleteIfExistsAsync es más seguro ya que no lanza excepción si no existe.
            await blobClient.DeleteIfExistsAsync();
        }
        catch (UriFormatException)
        {
            Console.WriteLine($"Advertencia: URL de archivo inválida: {urlCompleta}. No se pudo eliminar.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al intentar eliminar el archivo {urlCompleta} en Azure: {ex.Message}");
            throw;
        }
    }

    public async Task<string> SubirArchivoConNombreAsync(IFormFile archivo, string nombreContenedor, string nombreArchivo)
    {
        var contenedorCliente = _blobServiceClient.GetBlobContainerClient(nombreContenedor);
        await contenedorCliente.CreateIfNotExistsAsync();
        var blobCliente = contenedorCliente.GetBlobClient(nombreArchivo);

        var blobHttpHeaders = new BlobHttpHeaders
        {
            ContentType = archivo.ContentType
        };

        var blobUploadOptions = new BlobUploadOptions
        {
            HttpHeaders = blobHttpHeaders
        };

        using (var stream = archivo.OpenReadStream())
        {
            await blobCliente.UploadAsync(stream, blobUploadOptions);
        }

        return blobCliente.Uri.ToString();
    }

    public async Task<string> SubirArchivoAsync(IFormFile archivo, string nombreContenedor)
    {
        var contenedorCliente = _blobServiceClient.GetBlobContainerClient(nombreContenedor);
        await contenedorCliente.CreateIfNotExistsAsync();
        string nombreBlob = $"{Guid.NewGuid()}{Path.GetExtension(archivo.FileName)}";
        var blobCliente = contenedorCliente.GetBlobClient(nombreBlob);

        // 1. Definir los encabezados HTTP del blob
        var blobHttpHeaders = new BlobHttpHeaders
        {
            ContentType = archivo.ContentType
        };

        // 2. Opciones de subida, incluyendo los encabezados
        var blobUploadOptions = new BlobUploadOptions
        {
            HttpHeaders = blobHttpHeaders
        };
        using (var stream = archivo.OpenReadStream())
        {
            await blobCliente.UploadAsync(stream, blobUploadOptions);
        }
        return blobCliente.Uri.ToString();
    }
}
