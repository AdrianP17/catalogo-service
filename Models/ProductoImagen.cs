namespace catalogo.Models

{
    public class ProductoImagen
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public bool Principal { get; set; }
        public string Imagen { get; set; } = string.Empty;

        public Producto Producto { get; set; } = null!;
    }
}
