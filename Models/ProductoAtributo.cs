namespace catalogo.Models
{
    public class ProductoAtributo
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int AtributoValorId { get; set; }

        public Producto Producto { get; set; } = null!;
        public AtributoValor AtributoValor { get; set; } = null!;
    }
}
