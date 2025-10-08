namespace catalogo.Models
{
    public class VarianteAtributo
    {
        public int Id { get; set; }
        public int VarianteId { get; set; }
        public int AtributoValorId { get; set; }

        public Variante Variante { get; set; } = null!;
        public AtributoValor AtributoValor { get; set; } = null!;
    }
}
