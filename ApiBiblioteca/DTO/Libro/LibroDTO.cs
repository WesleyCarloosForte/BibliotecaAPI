using System.ComponentModel.DataAnnotations;

namespace ApiBiblioteca.DTO.Libro
{
    public class LibroDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Titulo { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Paginas { get; set; }
        public int AutorId { get; set; }
        public int GeneroId { get; set; }
    }
}
