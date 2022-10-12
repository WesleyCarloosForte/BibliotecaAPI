using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiBiblioteca.Data;
using ApiBiblioteca.Models;
using ApiBiblioteca.DTO.Libro;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace ApiBiblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly ApiLibrosContext _context;
        private readonly IMapper _mapper;
        public LibrosController(ApiLibrosContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;   
        }

        // GET: api/Libroes
        /// <summary>
        /// Devuelve un listado de todos los libros 
        /// El usuario debe estar autenticado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<LibroGetDTO>>> GetLibros()
        {
            var libros = await _context.Libros.Include("Genero").ToListAsync();
            var libroaGetDTO = _mapper.Map<List<LibroGetDTO>>(libros);
            return libroaGetDTO;
        }

        /// <summary>
        /// Devuelve un listado de todos los libros filtrando por titulo 
        /// Paginando cantidad maxima por pagina(maxItem) y numero de pagina(pageNumber)
        /// La paginacion empieza en 0
        /// El usuario debe estar autenticado
        /// </summary>
        /// <returns></returns>
        [HttpGet("filter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<LibroGetDTO>>> GetLibrosByTitulo(string titulo,int pageNumber,int maxItem )
        {
            if (pageNumber < 0 || maxItem < 0)
                return BadRequest("Elementos de paginacion negativos");

            var libros = await _context.Libros.Include("Genero").Where(libro=>libro.Titulo.Contains(titulo)).Skip(pageNumber*maxItem).Take(maxItem).ToListAsync();
            var libroaGetDTO = _mapper.Map<List<LibroGetDTO>>(libros);
            return libroaGetDTO;
        }

        /// <summary>
        /// Busca un libro por su Id (parámetro numérico de tipo entero)
        /// El usuario debe estar autenticado
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<LibroGetDTO>> GetLibro(int id)
        {
            var libro = await _context.Libros.Include("Genero").Where(x=>x.Id==id).FirstOrDefaultAsync();

            if (libro == null)
            {
                return NotFound();
            }
            
            return _mapper.Map<LibroGetDTO>(libro);
        }

        /// <summary>
        /// Modifica un libro previamente registrado 
        /// El usuario debe estar autenticado (rol administrador)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libroDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "admin")]
        public async Task<ActionResult<LibroGetDTO>> PutLibro(int id, LibroDTO libroDTO)

        {
            if (id != libroDTO.Id)
            {
                return BadRequest();
            }
            if (id != libroDTO.Id)
            {
                return BadRequest();
            }
            var libro =_mapper.Map<Libro>(libroDTO);
            _context.Entry(libro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return _mapper.Map<LibroGetDTO>(libro);
        }

        /// <summary>
        /// Registra un libro
        /// El usuario debe estar autenticado (rol administrador)
        /// </summary>
        /// <param name="libroDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "admin")]
        public async Task<ActionResult<LibroGetDTO>> PostLibro(LibroDTO libroDTO)
        {
            var libro = _mapper.Map<Libro>(libroDTO);

            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            libroDTO.Id = libro.Id;

            return CreatedAtAction("GetLibro", new { id = libro.Id }, libroDTO);
        }

        /// <summary>
        /// Eliminar un libro por su Id
        /// El usuario debe estar autenticado (rol administrador)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "admin")]
        public async Task<IActionResult> DeleteLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.Id == id);
        }
    }
}
