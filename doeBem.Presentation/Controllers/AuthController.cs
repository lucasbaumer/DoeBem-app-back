using BackendProjeto.Application.Services;
using doeBem.Application.DTOS;
using doeBem.Application.Interfaces;
using doeBem.Application.Services;
using doeBem.Core.Interfaces;
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
        private readonly IDonorService _donorService;

        public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> login, TokenService tokenService, IDonorService donorService)
        {
            _userManager = userManager;
            _login = login;
            _tokenService = tokenService;
            _donorService = donorService;
        }

        [HttpPost("Login")]
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


        [HttpPost("Register/Donor")]
        public async Task<ActionResult> CadastrarUsuario(DonorCreateDTO donorCreateDto)
        {
            try
            {
                await _donorService.RegisterDonor(donorCreateDto);
                return Ok("Doador cadastrado com sucesso!");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}