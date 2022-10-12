using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Autor:Persona
    {
        public List<Libro> Libros { get; set; }
    }
}