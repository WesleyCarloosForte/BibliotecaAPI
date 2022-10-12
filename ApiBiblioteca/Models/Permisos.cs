using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBiblioteca.Models
{
    public class Permisos
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public bool Leer { get; set; }
        public bool Escribir { get; set; }
        public bool Editar { get; set; }
        public bool Insertar { get; set; }
        public bool Eliminar { get; set; }
    }
}
