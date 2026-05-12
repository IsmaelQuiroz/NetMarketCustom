using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Errors;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    public class UsuarioController : BaseApiController
    {
        //userManager es el objeto que tiene acceso a todas las Entiades de Identity
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        //Token #
        private readonly ITokenService _tokenService;

        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly IGenericSeguridadRepository<Usuario> _seguridadRepository; //Generic Repository Pattern 3
        private readonly RoleManager<IdentityRole> _roleManager; //Roles 5

        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager,
            ITokenService tokenService, IMapper mapper, IPasswordHasher<Usuario> passwordHasher, 
            IGenericSeguridadRepository<Usuario> seguridadRepository, RoleManager<IdentityRole> roleManager) //Roles 5
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;  //Token #
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _seguridadRepository = seguridadRepository; //Generic Repository Pattern 4
            _roleManager = roleManager; //Roles 5
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDto>> Login(LoginDto loginDto)
        {
            
            var usuario = await _userManager.FindByEmailAsync(loginDto.Email);

            if (User == null)
            {
                return Unauthorized(new CodeErrorResponse(401));
            }

            var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, loginDto.Password, false);

            if (!resultado.Succeeded)
            {
                return Unauthorized(new CodeErrorResponse(401));
            }

            var roles = await _userManager.GetRolesAsync(usuario);

            return new UsuarioDto
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Username = usuario.UserName,
                Token = _tokenService.CreateToken(usuario, roles),  //Token #
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Imagen = usuario.Imagen,
                Admin = roles.Contains("ADMIN") ? true : false //Roles 7
            }; 
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioDto>> Registrar(RegistrarDto registroDto)
        {
            var usuario = new Usuario
            {
                Email = registroDto.Email,
                UserName = registroDto.Username,
                Nombre = registroDto.Nombre,
                Apellido = registroDto.Apellido
            };

            var resultado = await _userManager.CreateAsync(usuario, registroDto.Password);

            if (!resultado.Succeeded)
            {
                return BadRequest(new CodeErrorResponse(400));
            }

            return new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Token = _tokenService.CreateToken(usuario, null), //Token #
                Email = usuario.Email,
                Username = usuario.UserName,
                Admin = false
            };
        }



        [Authorize] //Cualquier usuario registrado tiene acceso a este metodo
        [HttpPut("actualizar/{id}")]
        public async Task<ActionResult<UsuarioDto>> Actualizar(string id, RegistrarDto registrarDto)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                return NotFound(new CodeErrorResponse(404, "El usuario no existe"));
            }

            usuario.Nombre = registrarDto.Nombre;
            usuario.Apellido = registrarDto.Apellido;
            usuario.Imagen = registrarDto.Imagen;

            if (!string.IsNullOrEmpty(registrarDto.Password))
            {
                usuario.PasswordHash = _passwordHasher.HashPassword(usuario, registrarDto.Password);
            }            

            var resultado = await _userManager.UpdateAsync(usuario);

            if (!resultado.Succeeded)
            {
                return BadRequest(new CodeErrorResponse(400, "No se pudo actualizar el usuario"));
            }

            var roles = await _userManager.GetRolesAsync(usuario);

            return new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Username = usuario.UserName,
                Token = _tokenService.CreateToken(usuario, roles),
                Imagen = usuario.Imagen,
                Admin = roles.Contains("ADMIN") ? true : false
            };

        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("pagination")] //es el texto que se agrega al final del endpoint del metodo
        public async Task<ActionResult<Pagination<UsuarioDto>>> GetUsuarios([FromQuery] UsuarioSpecificationParams usuarioParams)//devuelve un objeto tipo Pagination con data de tipo UsuarioDto,
        {//[FromQuery] porque los parámetros viajan dentro de la URL y estos datos son de tipo UsuarioSpecificationParams

            //las especificaciones ocupan los parámetros para crear la logica
            var spec = new UsuarioSpecification(usuarioParams);
            //Todos los filtros y la logica para los usuarios se basan en especificaciones,
            var usuarios = await _seguridadRepository.GetAllWithSpec(spec);//se pasa la spec para obtener la lista de usuarios

            //y luego se le pasa ese objeto especificación al repositorio que devulve la data de usuarios o del total de usuarios
            var specCount = new UsuarioForCountingSpecification(usuarioParams);
            var totalUsuarios = await _seguridadRepository.CountAsync(specCount); //se pasa el spec para obtener el Total de usuarios

            //para que redondee con el valor máximo de usuarios
            //ejemplo 1.3 a 2, y 5.3 a 6
            var rounded = Math.Ceiling(Convert.ToDecimal(totalUsuarios) / Convert.ToDecimal(usuarioParams.PageSize));
            var totalPages = Convert.ToInt32(rounded);

           
            //mapeo de una lista IReadOnlyList<Usuario> contra otra lista de UsuarioDto
            var data = _mapper.Map<IReadOnlyList<Usuario>, IReadOnlyList<UsuarioDto>>(usuarios); //este mapping debe registrarse dentro del MappingProfiles WebApi.Dtos

            //added by Iqs
            foreach (var usuarioDto in data)
            {
                var usuario = await _userManager.FindByIdAsync(usuarioDto.Id);
                var roles = await _userManager.GetRolesAsync(usuario);
                usuarioDto.Admin = roles.Contains("ADMIN") ? true : false;
            }


            //devuelve el objeto Pagination al cliente
            return Ok(
                new Pagination<UsuarioDto>
                {
                    Count = totalUsuarios,
                    Data = data,
                    PageCount = totalPages,
                    PageIndex = usuarioParams.PageIndex,
                    PageSize = usuarioParams.PageSize,
                }
            );
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("role/{id}")] //Roles 6
        public async Task<ActionResult<UsuarioDto>> UpdateRole(string id, RoleDto roleParam)
        {
            var role = await _roleManager.FindByNameAsync(roleParam.Nombre);
            if (role == null)
            {
                return NotFound(new CodeErrorResponse(404, "El rol no existe"));
            }

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                return NotFound(new CodeErrorResponse(404, "El usuario no existe"));
            }

            var usuarioDto = _mapper.Map<Usuario, UsuarioDto>(usuario);

            if (roleParam.Status)
            {
                var resultado = await _userManager.AddToRoleAsync(usuario, roleParam.Nombre);
                if (resultado.Succeeded)
                {
                    usuarioDto.Admin = true;
                }

                if (resultado.Errors.Any())
                {
                    if (resultado.Errors.Where(x => x.Code == "UserAlreadyRole").Any())
                    {
                        usuarioDto.Admin = true;
                    }
                }
            }
            else
            {
                var resultado = await _userManager.RemoveFromRoleAsync(usuario, roleParam.Nombre);
                if (resultado.Succeeded)
                {
                    usuarioDto.Admin = false;
                }
            }

            if (usuarioDto.Admin)
            {
                var roles = new List<string>();
                if (usuarioDto.Admin)
                {
                    roles.Add("ADMIN");
                    usuarioDto.Token = _tokenService.CreateToken(usuario, roles);
                }
                else
                {
                    usuarioDto.Token = _tokenService.CreateToken(usuario, null);
                }

            }

            return usuarioDto;
        }

        [Authorize(Roles ="ADMIN")]
        [HttpGet("account/{id}")]
        public async Task<ActionResult<UsuarioDto>> GetUsuarioById(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                return NotFound(new CodeErrorResponse(404, "el usuario no existe"));
            }

            var roles = await _userManager.GetRolesAsync(usuario);

            return new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Username = usuario.UserName,
                Imagen = usuario.Imagen,
                Admin = roles.Contains("ADMIN") ? true : false
            };
        }


        [Authorize] //No se le ponen parámetros porque el Token se envía dentro del Header del Request, ingresando gracias a un token
        [HttpGet]
        public async Task<ActionResult<UsuarioDto>> GetUsuario() //Metodo  para obtener los datos del usaurio en sesion, 
        {
            //estas lineas ya quedaron incluidas en le metodo estatico del USerManagerExtensions.cs
            //var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            //var usuario = await _userManager.FindByEmailAsync(email);

            //Obtiene la instancia del usuario desde el token que se envia dentro del Header del request
            var usuario = await _userManager.BuscarUsuarioAsync(HttpContext.User);

            var roles = await _userManager.GetRolesAsync(usuario);

            return new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Username = usuario.UserName,
                Imagen = usuario.Imagen,
                Token = _tokenService.CreateToken(usuario, roles),
                Admin = roles.Contains("ADMIN") ? true : false

            };
        }

        [Authorize]
        [HttpGet("emailvalido")]
        public async Task<ActionResult<bool>> ValidarEmail([FromQuery] string email)
        {
            var usuario = await _userManager.FindByEmailAsync(email);

            if (usuario == null) return false;

            return true;

        }

        [Authorize]
        [HttpGet("direccion")]
        public async Task<ActionResult<DireccionDto>> GetDireccion()
        {
            //estas lineas ya quedaron incluidas en le metodo estatico del USerManagerExtensions.cs
            //var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            //var usuario = await _userManager.FindByEmailAsync(email);

            var usuario = await _userManager.BuscarUsuarioConDireccionAsync(HttpContext.User);

            return _mapper.Map<Direccion, DireccionDto>(usuario.Direccion);
        }

        [Authorize]
        [HttpPut("direccion")]
        public async Task<ActionResult<DireccionDto>> UpdateDireccion(DireccionDto direccion)
        {
            var usuario = await _userManager.BuscarUsuarioConDireccionAsync(HttpContext.User);
            usuario.Direccion = _mapper.Map<DireccionDto, Direccion>(direccion);
            var resultado = await _userManager.UpdateAsync(usuario);
            if (resultado.Succeeded) return Ok(_mapper.Map<Direccion, DireccionDto>(usuario.Direccion));
            return BadRequest("No se pudo actualizar la dirección del usuario");


        }


    }
}
