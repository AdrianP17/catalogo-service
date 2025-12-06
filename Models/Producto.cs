namespace catalogo.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int? PromocionId { get; set; }

        public ICollection<ProductoImagen> ProductoImagenes { get; set; } = new List<ProductoImagen>();
        public ICollection<Variante> Variantes { get; set; } = new List<Variante>();
        public ICollection<ProductoAtributo> ProductoAtributos { get; set; } = new List<ProductoAtributo>();
        public Promocion? Promocion { get; set; }
    }
}
