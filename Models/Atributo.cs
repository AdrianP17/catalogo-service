namespace catalogo.Models
{
    public class Atributo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public ICollection<AtributoValor> AtributoValores { get; set; } = new List<AtributoValor>();
    }
}
