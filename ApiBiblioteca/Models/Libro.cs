using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBiblioteca.Models
{
    public class Libro
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
        [ForeignKey (nameof(AutorId))]
        public Autor Autor { get; set; }
        [ForeignKey(nameof(GeneroId))]
        public Genero Genero { get; set; }


    }
}
