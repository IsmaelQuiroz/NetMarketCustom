using Core.Entities;
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


        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)    
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                Token = "Este es el token del usuario",
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido
            };
        }


    }
}
