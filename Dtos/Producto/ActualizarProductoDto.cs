using System.ComponentModel.DataAnnotations;
using catalogo.Dtos.Variante;

namespace catalogo.Dtos.Producto
{
    public class ActualizarProductoDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        [MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        public int? PromocionId { get; set; }

        public ICollection<string> ImagenesExistentesUrls { get; set; } = [];
        public ICollection<IFormFile> NuevasImagenesArchivos { get; set; } = [];

        public ICollection<int> IdsAtributosValores { get; set; } = [];
    }
}
