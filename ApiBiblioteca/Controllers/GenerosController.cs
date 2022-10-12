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
using System.Collections;
using ApiBiblioteca.DTO.Genero;

namespace ApiBiblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private readonly ApiLibrosContext _context;
        private readonly IMapper _mapper;
        public GenerosController(ApiLibrosContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Generoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeneroDTO>>> GetGeneros()
        {
            try
            {
                var Generos = await _context.Generos.ToListAsync();
                return _mapper.Map<List<GeneroDTO>>(Generos);
            }
            catch (Exception)
            {
                 return new StatusCodeResult(500);
            }

        }

        // GET: api/Generoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GeneroDTO>> GetGenero(int id)
        {
            var genero = await _context.Generos.FindAsync(id);

            if (genero == null)
            {
                return NotFound();
            }

            return _mapper.Map<GeneroDTO> (genero);
        }

        // PUT: api/Generoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<GeneroDTO>> PutGenero(int id, GeneroDTO generoDTO)
        {
            if (id != generoDTO.Id)
            {
                return BadRequest();
            }
            var genero = _mapper.Map<Genero>(generoDTO);
            _context.Entry(genero).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeneroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return generoDTO;
        }

        // POST: api/Generoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GeneroDTO>> PostGenero(GeneroDTO generoDTO)
        {
            var genero = _mapper.Map<Genero>(generoDTO);

            _context.Generos.Add(genero);
            await _context.SaveChangesAsync();

            generoDTO.Id = genero.Id;

            return CreatedAtAction("GetGenero", new { id = generoDTO.Id }, generoDTO);
        }

        // DELETE: api/Generoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenero(int id)
        {
            var genero = await _context.Generos.FindAsync(id);
            if (genero == null)
            {
                return NotFound();
            }

            _context.Generos.Remove(genero);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GeneroExists(int id)
        {
            return _context.Generos.Any(e => e.Id == id);
        }
    }
}
