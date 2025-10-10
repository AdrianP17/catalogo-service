public interface IAlmacenadorArchivos
{
    Task<string> SubirArchivoAsync(IFormFile archivo, string contenedor);
}