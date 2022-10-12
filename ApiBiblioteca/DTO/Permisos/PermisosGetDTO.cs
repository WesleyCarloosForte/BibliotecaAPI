namespace ApiBiblioteca.DTO.Permisos
{
    public class PermisosGetDTO
    {
        public bool Leer { get; set; }
        public bool Escribir { get; set; }
        public bool Editar { get; set; }
        public bool Insertar { get; set; }
        public bool Eliminar { get; set; }
    }
}
