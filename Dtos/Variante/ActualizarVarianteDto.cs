using System.ComponentModel.DataAnnotations;

namespace catalogo.Dtos.Variante
{
    public class ActualizarVarianteDto
    {
        public int Id { get; set; }

        [Required]
        public string Sku { get; set; } = string.Empty; // SKU para la validación de unicidad

        [Required]
        [Range(0.01, 5000)]
        public decimal Precio { get; set; }

        public ICollection<int> IdsAtributosValores { get; set; } = [];

        public ICollection<string> ImagenesExistentesUrls { get; set; } = [];
        public ICollection<IFormFile> NuevasImagenesArchivos { get; set; } = [];
    }
}
