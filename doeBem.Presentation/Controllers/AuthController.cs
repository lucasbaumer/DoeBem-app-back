using BackendProjeto.Application.Services;
using doeBem.Application.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BackendProjeto.Presentation.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/")]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _login;
        private readonly TokenService _tokenService;

        public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> login, TokenService tokenService)
        {
            _userManager = userManager;
            _login = login;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> LogarUsuario(LoginDTO loginDto)
        {
            var usuario = await _userManager.FindByEmailAsync(loginDto.Email);
            var senha = await _userManager.CheckPasswordAsync(usuario, loginDto.Password);

            if (usuario != null && senha)
            {
                var token = await _tokenService.GenerateToken(usuario);
                return Ok(token);
            }

            return Unauthorized();
        }


        [HttpPost("cadastrar")]
        public async Task<ActionResult> CadastrarUsuario(LoginDTO loginDto)
        {
            var usuarioExistente = await _userManager.FindByEmailAsync(loginDto.Email);

            if (usuarioExistente != null)
            {
                throw new Exception("Email já cadastrado");
            }

            var usuario = new IdentityUser
            {
                UserName = loginDto.Email,
                Email = loginDto.Email
            };

            var result = await _userManager.CreateAsync(usuario, loginDto.Password);

            if (result.Succeeded)
            {
                return Ok("Usuario Cadastrado");
            }

            return BadRequest(result.Errors);
        }

    }
}