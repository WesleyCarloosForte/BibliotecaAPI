namespace ApiBiblioteca.DTO.Autenticacion
{
    public class RespuestaAutenticacionDTO
    {
        public string Token { get; set; }
        public string Rol { get; set; } = "User";
        public DateTime Expiracion { get; set; }
    }
}
