public interface IAlmacenadorArchivos
{
    Task<string> SubirArchivoAsync(IFormFile archivo, string contenedor);
    Task<string> SubirArchivoConNombreAsync(IFormFile archivo, string contenedor, string nombreArchivo);
    Task EliminarArchivoAsync(string url, string contenedor);
}
