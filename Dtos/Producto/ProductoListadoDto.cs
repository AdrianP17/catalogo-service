namespace catalogo.Dtos.Producto
{
    public class ProductoListadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal PrecioOriginal { get; set; }
        public decimal PrecioFinal { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public bool TienePromocion { get; set; }
    }
}
