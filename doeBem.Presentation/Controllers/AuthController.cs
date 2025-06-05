using BackendProjeto.Application.Services;
using doeBem.Application.DTOS;
using doeBem.Application.Interfaces;
using doeBem.Application.Services;
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
        public async Task<ActionResult> CadastrarUsuario(DonorCreateDTO donorCreateDto)
        {
            var usuarioExistente = await _userManager.FindByEmailAsync(donorCreateDto.Email);

            if (usuarioExistente != null)
            {
                throw new Exception("Email já cadastrado");
            }

            var usuario = new IdentityUser
            {
                UserName = donorCreateDto.Email,
                Email = donorCreateDto.Email,
                PhoneNumber = donorCreateDto.Phone
            };

            var result = await _userManager.CreateAsync(usuario, donorCreateDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            try
            {
                await _donorService.RegisterDonor(new DonorCreateDTO
                {
                    Name = donorCreateDto.Name,
                    Email = donorCreateDto.Email,
                    Phone = donorCreateDto.Phone,
                    Cpf = donorCreateDto.Cpf,
                    DateOfBirth = donorCreateDto.DateOfBirth
                }); 
            }
            catch(Exception err)
            {
                await _userManager.DeleteAsync(usuario);
                return BadRequest("Erro ao cadastrar doador!!" + err.Message);
            }

            return Ok("Doador cadastrado com sucesso!");
        }
    }
}