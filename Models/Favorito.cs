namespace catalogo.Models
{
    public class Favorito
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int IdCliente { get; set; }

        public Producto Producto { get; set; } = null!;
    }
}
