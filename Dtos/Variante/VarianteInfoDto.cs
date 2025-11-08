using catalogo.Dtos.Atributo;

namespace catalogo.Dtos.Variante
{
    public class VarianteInfoDto
    {
        public int Id { get; set; }
        public string Sku { get; set; } = "";
        public string Imagen { get; set; } = "";
        public ICollection<AtributoValorDto> Atributos { get; set; } = [];
    }
}
