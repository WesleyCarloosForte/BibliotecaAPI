using ApiBiblioteca.DTO.UsuarioDTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBiblioteca.Models
{
    public class Usuario:Persona
    {
        public string IdentityUserId { get; set; }
        [NotMapped]
        public IdentityUser IdentityUser { get; set; }
    }


}
