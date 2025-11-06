namespace catalogo.Dtos.Producto
{
    public class ProductoDetalleDto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Imagen { get; set; } = string.Empty;
    }
}
