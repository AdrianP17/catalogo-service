using catalogo.Dtos.Atributo;

namespace catalogo.Dtos.Variante
{
    public class VarianteDto
    {
        public int Id { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioConDescuento { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public bool TieneDescuento { get; set; }
        public ICollection<string> Imagenes { get; set; } = new List<string>();
        public ICollection<AtributoValorDto> Atributos { get; set; } = new List<AtributoValorDto>();
    }
}
