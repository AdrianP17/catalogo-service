using System.ComponentModel.DataAnnotations;

namespace catalogo.Dtos.Atributo
{
    public class CrearAtributoValorDto
    {
        [Required]
        [MaxLength(50)]
        public string Valor { get; set; } = string.Empty;
    }
}
