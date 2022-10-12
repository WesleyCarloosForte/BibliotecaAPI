using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiBiblioteca.Data;
using ApiBiblioteca.Models;
using ApiBiblioteca.DTO;
using ApiBiblioteca.DTO.UsuarioDTO;
using AutoMapper;

namespace ApiBiblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApiLibrosContext _context;
        private readonly IMapper _mapper;

        public UsuariosController(ApiLibrosContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuarios()
        {

            var usuario= await  _context.Usuarios.Include("Permisos").ToListAsync();

            var usuarioDTO=_mapper.Map<List<UsuarioDTO>>(usuario);
            
            return usuarioDTO;
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.Include("Permisos").Where(x=>x.Id==id).FirstOrDefaultAsync();



            if (usuario == null)
            {
                return NotFound();
            }
            var usuarioDTO = _mapper.Map<UsuarioDTO>(usuario);
            return usuarioDTO;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<UsuarioDTO>> PutUsuario(int id, UsuarioDTO usuarioDTO)
        {
            if (id != usuarioDTO.Id)
            {
                return BadRequest();
            }
            var usuario = _mapper.Map<Usuario>(usuarioDTO);

            

            var permisos = _mapper.Map<Permisos>(usuarioDTO.Permisos);
            var permisoId=GetPermisosId(usuarioDTO.Id);

            if(permisoId==0)
            {
                return NotFound();
            }

            permisos.Id = permisoId;

            usuario.PermisosId=permisoId;
            usuario.Permisos=permisos;

            _context.ChangeTracker.Clear();
            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return usuarioDTO;
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(UsuarioDTO usuarioDTO)
        {
            var usuario = _mapper.Map<Usuario>(usuarioDTO); 

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            usuarioDTO.Id = usuario.Id;

            return CreatedAtAction("GetUsuario", new { id = usuarioDTO.Id }, usuarioDTO);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
        private int GetPermisosId(int usuariId)
        {
            var usuario = _context.Usuarios.Find(usuariId);
            if (usuario == null)
            {
                return 0;
            }
            return usuario.PermisosId;
        }
    }
}
