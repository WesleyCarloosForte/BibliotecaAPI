using ApiBiblioteca.DTO.Autor;
using ApiBiblioteca.DTO.Genero;
using ApiBiblioteca.DTO.Libro;
using ApiBiblioteca.DTO.Permisos;
using ApiBiblioteca.DTO.UsuarioDTO;
using ApiBiblioteca.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

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


            CreateMap<UsuarioDTO, Usuario>();
            CreateMap<Usuario, UsuarioDTO>();

            CreateMap<UsuarioIdentityDTO, IdentityUser>();
            CreateMap<IdentityUser, UsuarioIdentityDTO>();

            CreateMap<UsuarioDTO, Usuario>();
            CreateMap<Usuario, UsuarioDTO>();

            CreateMap<IdentityUser, Usuario>();
            CreateMap<Usuario, IdentityUser>();

        }
        
    }
}
