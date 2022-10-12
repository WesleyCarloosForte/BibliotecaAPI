using ApiBiblioteca.DTO.Autor;
using ApiBiblioteca.DTO.Genero;
using ApiBiblioteca.DTO.Libro;
using ApiBiblioteca.DTO.Permisos;
using ApiBiblioteca.DTO.UsuarioDTO;
using ApiBiblioteca.Models;
using AutoMapper;

namespace ApiBiblioteca.AutoMapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GeneroDTO, Genero>();
            CreateMap<Genero, GeneroDTO>();

            CreateMap<AutorDTO, Autor>();
            CreateMap<Autor, AutorDTO>();

            CreateMap<AutorGetDTO, Autor>();
            CreateMap<Autor, AutorGetDTO>();

            CreateMap<LibroDTO, Libro>();
            CreateMap<Libro, LibroDTO>();

            CreateMap<LibroGetDTO, Libro>();
            CreateMap<Libro, LibroGetDTO>();

            CreateMap<PermisosDTO, Permisos>();
            CreateMap<Permisos, PermisosDTO>();

            CreateMap<PermisosGetDTO, Permisos>();
            CreateMap<Permisos, PermisosGetDTO>();

            CreateMap<UsuarioDTO, Usuario>();
            CreateMap<Usuario, UsuarioDTO>();




        }
        
    }
}
