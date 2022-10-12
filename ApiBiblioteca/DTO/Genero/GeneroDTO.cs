using System.ComponentModel.DataAnnotations;

namespace ApiBiblioteca.DTO.Genero
{
    public class GeneroDTO
    {
        [Key]
        public int Id { get; set; }
        [Required()]
        [MaxLength(100)]
        [MinLength(1)]
        public string Descripcion { get; set; }
    }
}
