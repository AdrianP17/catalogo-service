using System.ComponentModel.DataAnnotations;

namespace catalogo.Dtos.Producto
{
    public class CrearProductoDto
    {
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        [MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty;
        public ICollection<int> IdsCategorias { get; set; } = [];
        public ICollection<IFormFile> Imagenes { get; set; } = new List<IFormFile>();
    }
}
