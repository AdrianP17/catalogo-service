using System.ComponentModel.DataAnnotations;

namespace catalogo.Dtos.Variante
{
    public class CrearVarianteDto
    {
        [Required]
        public string Sku { get; set; } = string.Empty;
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public ICollection<int> IdsAtributosValores { get; set; } = [];
        [Required]
        public ICollection<IFormFile> Imagenes { get; set; } = [];
    }
}
