using System.ComponentModel.DataAnnotations;

namespace ApiBiblioteca.Models
{
    public class Autor:Persona
    {
        public List<Libro> Libros { get; set; }
    }
}