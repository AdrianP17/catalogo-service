using System.ComponentModel.DataAnnotations;

namespace catalogo.Dtos.Atributo
{
    public class ActualizarAtributoDto
    {
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;
    }
}
