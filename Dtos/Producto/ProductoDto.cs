namespace catalogo.Dtos.Producto
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool TienePromocion { get; set; }
        public int? IdPromocion { get; set; }
    }
}
