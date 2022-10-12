using ApiBiblioteca.DTO.Permisos;
using System.ComponentModel.DataAnnotations;

namespace ApiBiblioteca.DTO.UsuarioDTO
{
    public class UsuarioDTO
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
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }
        [Required]
        [MaxLength(30)]
        [MinLength(4)]
        public string Login { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(6)]
        public string Password { get; set; }
        public PermisosGetDTO Permisos { get; set; }

    }
}
