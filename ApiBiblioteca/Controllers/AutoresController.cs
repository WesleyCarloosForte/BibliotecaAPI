using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiBiblioteca.Data;
using ApiBiblioteca.Models;
using AutoMapper;
using ApiBiblioteca.DTO.Autor;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace ApiBiblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly ApiLibrosContext _context;
        private readonly IMapper _mapper;

        public AutoresController(ApiLibrosContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

  
        /// <summary>
        /// Listar los autores con sus livros 
        /// El usuario debe estar autenticado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<AutorGetDTO>>> GetAutores()
        {
            var autores= await _context.Autores.Include(autor => autor.Libros).ThenInclude(libro => libro.Genero).ToListAsync();
            return _mapper.Map<List<AutorGetDTO>>(autores);
        }

        /// <summary>
        /// Lista un autor filtrando por Id 
        /// El usuario debe estar autenticado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AutorGetDTO>> GetAutor(int id)
        {
            var autor = await _context.Autores.Include(autor => autor.Libros).ThenInclude(libro => libro.Genero).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (autor == null)
            {
                return NotFound();
            }

            return _mapper.Map<AutorGetDTO>(autor); ;
        }

        /// <summary>
        /// Modificar un autor
        /// El usuario debe estar autenticado (rol administrador)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="autorDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "admin")]
        public async Task<ActionResult<AutorDTO>> PutAutor(int id, AutorDTO autorDTO)
        {
            if (id != autorDTO.Id)
            {
                return BadRequest();
            }
            var autor= _mapper.Map<Autor>(autorDTO);
            _context.Entry(autor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AutorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return autorDTO;
        }
        /// <summary>
        /// Registrar un autor
        /// El usuario debe estar autenticado (rol administrador)
        /// </summary>
        /// <param name="autorDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "admin")]
        public async Task<ActionResult<AutorDTO>> PostAutor(AutorDTO autorDTO)
        {
            var autor = _mapper.Map<Autor>(autorDTO);
            _context.Autores.Add(_mapper.Map<Autor>(autor));
            await _context.SaveChangesAsync();

            autorDTO.Id = autor.Id;

            return CreatedAtAction("GetAutor", new { id = autor.Id }, autorDTO);
        }

        /// <summary>
        /// Eliminar un autor
        /// El usuario debe estar autenticado (rol administrador)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "admin")]
        public async Task<IActionResult> DeleteAutor(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }

            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AutorExists(int id)
        {
            return _context.Autores.Any(e => e.Id == id);
        }
    }
}
