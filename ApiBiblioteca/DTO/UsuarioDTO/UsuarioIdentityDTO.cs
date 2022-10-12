using System.ComponentModel.DataAnnotations;

namespace ApiBiblioteca.DTO.UsuarioDTO
{
    public class UsuarioIdentityDTO
    {
        [EmailAddress]
        [Required]
        [MinLength(6)]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        public string Password { get; set; }

    }
}
