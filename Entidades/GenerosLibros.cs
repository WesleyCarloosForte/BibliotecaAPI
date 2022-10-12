using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class GenerosLibros
    {
        [Key]
        public int Id { get; set; }
        public int GeneroId { get; set; }
        public int LibroId { get; set; }
        [ForeignKey(nameof(GeneroId))]
        public Genero Genero { get; set; }
        [ForeignKey(nameof(LibroId))]
        public Libro Libro { get; set; }
    }
}
