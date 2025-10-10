namespace catalogo.Models
{
    public class VarianteImagen
    {
        public int Id { get; set; }
        public int VarianteId { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public Variante Variante { get; set; } = null!;
    }
}
