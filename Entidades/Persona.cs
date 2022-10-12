using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Persona
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string Nombre { get; set; }
        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string Apellido { get; set; }
        [Required]
        public DateOnly FechaNacimiento { get; set; }
    }
}
