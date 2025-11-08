public interface IAlmacenadorArchivos
{
    Task<string> SubirArchivoAsync(IFormFile archivo, string contenedor);
    Task EliminarArchivoAsync(string url, string contenedor);
}
