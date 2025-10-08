using System.ComponentModel.DataAnnotations;

namespace catalogo.Dtos.Variante
{
    public class CrearVarianteDto
    {
        [Required]
        public string Sku { get; set; } = string.Empty;
        [Required]
        [Range(0.01, 5000)]
        public decimal Precio { get; set; }
        public ICollection<string> Imagenes { get; set; } = new List<string>();
        [Required]
        public ICollection<int> IdsAtributosValores { get; set; } = new List<int>();
    }
}
