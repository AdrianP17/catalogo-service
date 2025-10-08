namespace catalogo.Dtos.Atributo
{
    public class AtributoValorDetalleDto
    {
        public int Id { get; set; }
        public int AtributoId { get; set; }
        public string Valor { get; set; } = string.Empty;
        public string? NombreAtributo { get; set; }
    }
}