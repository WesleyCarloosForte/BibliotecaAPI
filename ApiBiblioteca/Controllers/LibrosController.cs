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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroGetDTO>>> GetLibros()
        {
            var libros = await _context.Libros.Include("Genero").ToListAsync();
            var libroaGetDTO = _mapper.Map<List<LibroGetDTO>>(libros);
            return libroaGetDTO;
        }

        // GET: api/Libroes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LibroGetDTO>> GetLibro(int id)
        {
            var libro = await _context.Libros.Include("Genero").Where(x=>x.Id==id).FirstOrDefaultAsync();

            if (libro == null)
            {
                return NotFound();
            }
            
            return _mapper.Map<LibroGetDTO>(libro);
        }

        // PUT: api/Libroes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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

        // POST: api/Libroes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LibroGetDTO>> PostLibro(LibroDTO libroDTO)
        {
            var libro = _mapper.Map<Libro>(libroDTO);

            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            libroDTO.Id = libro.Id;

            return CreatedAtAction("GetLibro", new { id = libro.Id }, libroDTO);
        }

        // DELETE: api/Libroes/5
        [HttpDelete("{id}")]
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
