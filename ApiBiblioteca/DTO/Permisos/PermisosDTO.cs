using System.ComponentModel.DataAnnotations;

namespace ApiBiblioteca.DTO.Permisos
{
    public class PermisosDTO
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
