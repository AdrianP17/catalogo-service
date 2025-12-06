using catalogo.Dtos.Atributo;
using catalogo.Dtos.Promocion;
using catalogo.Dtos.Variante;

namespace catalogo.Dtos.Producto
{
    public class ProductoDetalleDto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public PromocionDto? Promocion { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public ICollection<VarianteDto> Variantes { get; set; } = new List<VarianteDto>();
        public ICollection<AtributoValorDetalleDto> Atributos { get; set; } = new List<AtributoValorDetalleDto>();
    }
}
