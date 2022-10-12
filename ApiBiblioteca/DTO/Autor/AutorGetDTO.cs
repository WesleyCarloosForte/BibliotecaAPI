using ApiBiblioteca.DTO.Libro;

namespace ApiBiblioteca.DTO.Autor
{
    public class AutorGetDTO:AutorDTO
    {
        public IEnumerable<LibroGetDTO> Libros { get; set; }
    }
}
