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
    [ApiController]
    [Route("api/")]
    ///<sumary>
    ///realiza o Login do Doador
    ///</sumary>
    ///<returns>Token JWT de acesso</returns>
    public class LoginController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _login;
        private readonly TokenService _tokenService;
        private readonly IDonorService _donorService;
        private readonly IAdminService _adminService;

        public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> login, TokenService tokenService, IDonorService donorService, IAdminService adminService)
        {
            _userManager = userManager;
            _login = login;
            _tokenService = tokenService;
            _donorService = donorService;
            _adminService = adminService;
        }

        /// <summary>
        /// Realiza o login do Doador ou Administrador
        /// </summary>
        /// <returns>Realiza o login com a chave JWT e retorna mensagem de sucesso</returns>
        /// <param name="loginDto">Objeto contendo os dados do login do doador</param>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     POST api/Login
        ///     
        /// Exemplo de resposta: 
        /// 
        ///     {
        ///        "email": "joao@gmail.com",
        ///        "password": "SenhaSegura123@"
        ///     }
        /// </remarks>
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Realiza o cadastro do doador
        /// </summary>
        /// <param name="donorCreateDto">Objeto contendo os dados do doador</param>
        /// <returns>Cadastra o doador no banco de dados e retorna mensagem de sucesso</returns>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     POST api/Register/Donor
        ///     
        ///  Exemplo de Resposta: 
        ///  
        ///     {
        ///        "name": "João Silva",
        ///        "email": "joao@gmail.com",
        ///        "cpf": "999.999.999-99",
        ///        "phone": "(11)99999-9999",
        ///        "phone": "yyyy-MM-dd" (ano-mês-dia),
        ///        "password": "SenhaSegura123@"
        ///     }
        ///     
        /// </remarks>
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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


        /// <summary>
        /// Realiza o cadastro de um administrador
        /// </summary>
        /// <param name="adminCreateDto">Objeto contendo os dados do administrador</param>
        /// <returns>Cadastra o administrador no banco de dados e retorna mensagem de sucesso</returns>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     POST api/Register/Admin
        ///     
        ///  Exemplo de Resposta: 
        ///  
        ///     {
        ///        "name": "Administrador",
        ///        "email": "admin@gmail.com",
        ///        "password": "SenhaSegura123@"
        ///     }
        ///     
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("Register/Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CadastrarAdministrador(AdminCreateDTO adminCreateDto)
        {
            try
            {
                await _adminService.RegisterAdmin(adminCreateDto);
                return Ok("Administrador cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}