using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Usuario:Persona
    {
        [Required]
        [MaxLength(30)]
        [MinLength(4)]
        public string Login { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(6)]
        public string Password { get; set; }
        public int PermisosId { get; set; }
        [ForeignKey(nameof(PermisosId))]
        public Permisos Permisos { get;set;}
    }


}
