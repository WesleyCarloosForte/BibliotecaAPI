using System.ComponentModel.DataAnnotations;

namespace ApiBiblioteca.DTO.UsuarioDTO
{
    public class UsuarioGetDTO
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
        public string Rol { get; set; }
    }
}
