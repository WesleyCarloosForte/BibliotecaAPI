using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBiblioteca.Models
{
    public class Genero
    {
        [Key]
        public int Id { get; set; }
        [Required ()]
        [MaxLength(100)]
        [MinLength(1)]
        public string Descripcion { get; set; }
        public List<Libro> Libros { get; set; }
    }
}
