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
using ApiBiblioteca.DTO.Autenticacion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.ComponentModel;

namespace ApiBiblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ApiLibrosContext _context;
        private readonly SignInManager<IdentityUser> _SignInManager;


        public UsuariosController(ApiLibrosContext context, UserManager<IdentityUser> userManager, IConfiguration configuration, IMapper mapper, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _SignInManager = signInManager;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioGetDTO>>> GetUsuarios()
        {

            var usuario = await _context.Usuarios.Join(_context.UserClaims,user=>user.IdentityUserId,cl=>cl.UserId,(user,clamin) => new UsuarioGetDTO
            {
                Nombre=user.Nombre,
                Apellido=user.Apellido,
                FechaNacimiento=user.FechaNacimiento,
                Id=user.Id,
                Rol=clamin.ClaimType == "admin" ? "Administrador" : "Usurio"
            }).ToListAsync();
            

            return usuario;
        }
        /// <summary>
        /// Devuelve un listado de todos los usuarios filtrando por nombre 
        /// Paginando cantidad maxima por pagina (maxItem) y numero de pagina(pageNumber)
        /// La paginacion empieza en 0
        /// El usuario debe estar autenticado (rol administrador)
        /// </summary>
        /// <returns></returns>
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<UsuarioGetDTO>>> GetUsuariosByName(string nombre, int pageNumber, int maxItem)
        {
            if (pageNumber < 0 || maxItem < 0)
                return BadRequest("Elementos de paginacion negativos");

            var usuario = await _context.Usuarios.Join(_context.UserClaims, user => user.IdentityUserId, cl => cl.UserId, (user, clamin) => new UsuarioGetDTO
            {
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                FechaNacimiento = user.FechaNacimiento,
                Id = user.Id,
                Rol = clamin.ClaimType == "admin" ? "Administrador" : "Usurio"
            }).Where(user=>user.Nombre.Contains(nombre ))
            .Skip(maxItem*pageNumber)
            .Take(maxItem)
            .ToListAsync();


            return usuario;
        }
        /// <summary>
        /// Devuelve un listado de todos los usuarios filtrando por Id
        /// El usuario debe estar autenticado (rol administrador)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UsuarioGetDTO>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.Join(_context.UserClaims, user => user.IdentityUserId, cl => cl.UserId, (user, clamin) => new UsuarioGetDTO
            {
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                FechaNacimiento = user.FechaNacimiento,
                Id = user.Id,
                Rol = clamin.ClaimType == "admin" ? "Administrador" : "Usurio"
            }).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound();
            }
            
            return usuario;
        }
        /// <summary>
        /// Modifica un usuario previamente registrado 
        /// El usuario debe estar autenticado (rol administrador o usuarios no administrador dueño de los datos)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usuarioDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UsuarioDTO>> PutUsuario(int id, UsuarioDTO usuarioDTO)
        {
            if (id != usuarioDTO.Id)
            {
                return BadRequest();
            }
            var usuario = _mapper.Map<Usuario>(usuarioDTO);
            var claims = User.Claims.ToList();


            if (!IsPermitirAdmin() || !IsPermitirUsurio(usuarioDTO.IdentityUser.Email))
                return StatusCode(403);

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
        /// <summary>
        /// Elimina un usuario usando su Id
        /// El usuario debe estar autenticado (rol administrador)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "admin")]
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
        /// <summary>
        /// Permite a un usuario registrase
        /// Caso 1: primer usuario a registrarse sera el administrador
        /// Caso 2: si ya hay un usuario registrado, sera un usuario normal
        /// Permite al usuario autenticarse y recibir su token(si aun no estaba registrado)
        /// </summary>
        /// <param name="usuarioDTO"></param>
        /// <returns></returns>
        [HttpPost("registrar/login")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> RegistrarUsuarioAndLogin(UsuarioDTO usuarioDTO)
        {
            try
            {
                var usuarioIdentity = new IdentityUser { UserName = usuarioDTO.IdentityUser.Email, Email = usuarioDTO.IdentityUser.Email };

                var resultado = await _userManager.CreateAsync(usuarioIdentity);






                if (resultado.Succeeded)
                {
                    var usuario = _mapper.Map<Usuario>(usuarioDTO);

                    usuario.IdentityUserId = usuarioIdentity.Id;

                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();

                    var respuesta = GetToken(usuarioDTO.IdentityUser);

                    if (await IsPrimerRegistro())
                    {
                        await SetAdminPolicy(usuarioIdentity.Email);
                        respuesta.Rol = "Administrador";
                    }
                    else
                    {
                        await SetUserPolicy(usuarioIdentity.Email);
                        respuesta.Rol = "Usuario";
                    }


                    return respuesta;
                }
                else
                {
                    return BadRequest(resultado.Errors);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Permite al usuario autenticarse y recibir su token
        /// </summary>
        /// <param name="usuarioIdentityDTO"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Login(UsuarioIdentityDTO usuarioIdentityDTO)
        {
            var resultado = await _SignInManager.PasswordSignInAsync(usuarioIdentityDTO.Email, usuarioIdentityDTO.Password, isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return GetToken(usuarioIdentityDTO);
            }
            else
            {
                return BadRequest();
            }
        }
        private RespuestaAutenticacionDTO GetToken(UsuarioIdentityDTO usuarioIdentityDTO)
        {
            var claims = new List<Claim>()
            {
                new Claim ("email",usuarioIdentityDTO.Email)
            };

            var keyJwt = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["keyJwt"]));
            var credentials = new SigningCredentials(keyJwt, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(5);

            var securetyToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: credentials);

            return new RespuestaAutenticacionDTO
            {
                Token = $"Bearer {new JwtSecurityTokenHandler().WriteToken(securetyToken)}",
                Expiracion = expiration,

            };

        }
        /// <summary>
        /// Permite convertir un usuario normal en administrador, el parametro de busqueda pra convertir al usuario sera su email
        /// El usuario debe estar autenticado (rol administrador o usuarios no administrador dueño de los datos)
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "admin")]
        public async Task<ActionResult> DarPermisosAdmin(string email)
        {
            try
            {
                await SetAdminPolicy(email);

                return NoContent();
            }
            catch (Exception)
            {

                throw;
            }

        }
        private async Task SetAdminPolicy(string email)
        {
            var admin = await _userManager.FindByEmailAsync(email);
            await _userManager.AddClaimAsync(admin, new Claim("admin", "1"));
        }
        private async Task SetUserPolicy(string email)
        {
            var admin = await _userManager.FindByEmailAsync(email);
            await _userManager.AddClaimAsync(admin, new Claim("user", "2"));
        }
        private bool IsPermitirUsurio(string email) => User.Claims.Contains(new Claim("email", email));

        private  bool IsPermitirAdmin() => User.Claims.Contains(new Claim("admin", "1"));
        private async Task<bool> IsPrimerRegistro() => await _context.Users.CountAsync() == 1;
        /*
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
        }*/
    }
}
