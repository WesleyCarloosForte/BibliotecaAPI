using ApiBiblioteca.DTO.Autor;
using ApiBiblioteca.DTO.Genero;
using ApiBiblioteca.Models;
using System.ComponentModel.DataAnnotations;

namespace ApiBiblioteca.DTO.Libro
{
    public class LibroGetDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Titulo { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Paginas { get; set; }
        public GeneroDTO Genero { get; set; }
    }
}
