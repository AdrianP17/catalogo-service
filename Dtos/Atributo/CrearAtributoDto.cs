using System.ComponentModel.DataAnnotations;

namespace catalogo.Dtos.Atributo
{
    public class CrearAtributoDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Nombre { get; set; } = string.Empty;
    }
}
