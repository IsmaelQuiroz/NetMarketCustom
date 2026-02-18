using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Errors;

namespace WebApi.Controllers
{
    public class UsuarioController : BaseApiController
    {
        //userManager es el objeto que tiene acceso a todas las Entiades de Identity
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        //Token #
        private readonly ITokenService _tokenService;

        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ITokenService tokenService)    
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;  //Token #
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDto>> Login(LoginDto loginDto)
        {
            var usuario = await _userManager.FindByEmailAsync(loginDto.Email);

            if(User == null)
            {
                return Unauthorized(new CodeErrorResponse(401));
            }
            
            var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, loginDto.Password, false);

            if (!resultado.Succeeded)
            {
                return Unauthorized(new CodeErrorResponse(401));
            }

            return new UsuarioDto
            {
                Email = usuario.Email,
                Username = usuario.UserName,
                Token = _tokenService.CreateToken(usuario),  //Token #
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido
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
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Token = _tokenService.CreateToken(usuario), //Token #
                Email = usuario.Email,
                Username = usuario.UserName
            };
        }
    }
}
