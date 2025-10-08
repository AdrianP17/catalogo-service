namespace catalogo.Models
{
    public class AtributoValor
    {
        public int Id { get; set; }
        public int AtributoId { get; set; }
        public string Valor { get; set; } = string.Empty;

        public Atributo Atributo { get; set; } = null!;
    }
}
