
using ApiBiblioteca.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace ApiBiblioteca.Data
{
    public class ApiLibrosContext:IdentityDbContext
    {
        public string DbPath { get; }
        public ApiLibrosContext (DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Persona>()
                .HasDiscriminator<string>("persona_Type")
                .HasValue<Persona>("persona_base")
                .HasValue<Autor>("autor_base")
                .HasValue<Usuario>("usuario_base");
        }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Autor> Autores { get; set; }
       
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ApiBiblioteca.Models.Persona> Persona { get; set; }

    }
}
