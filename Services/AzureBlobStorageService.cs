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