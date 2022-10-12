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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

        /// <summary>
        /// Permite listar los generos
        /// El usuario debe estar autenticado (rol administrador)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "admin")]
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

        /// <summary>
        /// Permite listar un genero filtrando por su Id
        /// El usuario debe estar autenticado (rol administrador)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "admin")]
        public async Task<ActionResult<GeneroDTO>> GetGenero(int id)
        {
            var genero = await _context.Generos.FindAsync(id);

            if (genero == null)
            {
                return NotFound();
            }

            return _mapper.Map<GeneroDTO> (genero);
        }
        /// <summary>
        /// Permite Modificar un genero
        /// El usuario debe estar autenticado (rol administrador)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="generoDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "admin")]
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

        /// <summary>
        /// Permite Registrar un genero
        /// El usuario debe estar autenticado (rol administrador)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="generoDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "admin")]
        public async Task<ActionResult<GeneroDTO>> PostGenero(GeneroDTO generoDTO)
        {
            var genero = _mapper.Map<Genero>(generoDTO);

            _context.Generos.Add(genero);
            await _context.SaveChangesAsync();

            generoDTO.Id = genero.Id;

            return CreatedAtAction("GetGenero", new { id = generoDTO.Id }, generoDTO);
        }

        /// <summary>
        /// Permite Eliminar un genero
        /// El usuario debe estar autenticado (rol administrador)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="generoDTO"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "admin")]
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
