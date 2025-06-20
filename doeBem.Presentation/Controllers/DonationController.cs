using doeBem.Application.DTOS;
using doeBem.Application.Interfaces;
using doeBem.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace doeBem.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DonationController : ControllerBase
    {
        private readonly IDonationService _donationService;
        public DonationController(IDonationService donationService)
        {
            _donationService = donationService;
        }

        /// <summary>
        /// Realiza o retorno de todas as doações registradas no sistema
        /// </summary>
        /// <returns>Retorna todas as doações</returns>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     GET api/Donation
        ///     
        /// Exemplo de resposta: 
        /// 
        ///     [
        ///         {
        ///             "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "valor": 1000.00,
        ///             "data": "2025-04-20",
        ///             "idDoador": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "nomeDoador": "Carlos",
        ///             "idHospital": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "nomeHospital": "Hospital Pequeno Príncipe"
        ///         },
        ///         {
        ///             "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "valor": 1240.00,
        ///             "data": "2025-04-20",
        ///             "idDoador": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "nomeDoador": "Pedro",
        ///             "idHospital": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "nomeHospital": "Hospital IPO"
        ///         }
        ///     ]
        /// </remarks>

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonationDTO>>> GetAllDonations()
        {
            var donations = await _donationService.GetAllAsync();

            if (donations == null)
            {
                return NotFound(new { message = "Nenhuma doação encontrada!" });
            }

            if (!donations.Any())
            {
                return NotFound(new { message = "A lista de doações está vazia" });
            }

            return Ok(donations);
        }

        /// <summary>
        /// Realiza o retorno da doação com o id passado 
        /// </summary>
        /// <param name="id">ID da doação que será retornada</param>
        /// <returns>Retorna uma doação pelo id informado</returns>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     GET api/Donation/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     
        /// Exemplo de resposta:
        ///
        ///     {
        ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "valor": 1000.00,
        ///         "data": "2025-04-20",
        ///         "idDoador": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "nomeDoador": "Carlos",
        ///         "idHospital": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "nomeHospital": "Hospital Pequeno Príncipe"
        ///     }
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Donation>> GetDonationById(Guid id)
        {
            var donation = await _donationService.GetByIdAsync(id);
            if (donation == null)
            {
                return NotFound(new { message = "Nenhuma doação foi encontrada" });
            }

            return Ok(donation);
        }

        /// <summary>
        /// Realiza o cadastro de uma doação no sistema
        /// </summary>
        /// <param name="donationCreateDto">Objeto contendo os dados da doação</param>
        /// <returns>Cadastra uma doação no sistema</returns>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     POST api/Donation
        ///     
        /// Exemplo de corpo da requisição:
        /// 
        ///     {
        ///         "valor": 1000.00,
        ///         "data": "2025-04-20",
        ///         "idDoador": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "idHospital": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///     }
        ///     
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> RegisterDonation(DonationCreateDTO donationCreateDto)
        {
            try
            {
                var donationId = await _donationService.RegisterDonation(donationCreateDto);
                var donationResponse = await _donationService.GetByIdAsync(donationId);

                return CreatedAtAction(nameof(GetDonationById), new { id = donationId }, donationResponse);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = $"Erro ao cadastrar doação: {e.Message}" });
            }
        }

        /// <summary>
        /// Realiza a edição de uma doação no sistema
        /// </summary>
        /// <param name="id">id da doação que pretende editar</param>
        /// <param name="donationUpdateDto">Objeto contendo os dados do doador</param>
        /// <returns>edita uma doação pelo id que foi informado</returns>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     PUT api/Donation/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     
        /// Exemplo de resposta: 
        /// 
        ///     {
        ///        "Valor": "1000.00",
        ///        "Data": "2025-04-20" (ano-mês-dia),
        ///        "ID do Doador": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///        "ID do Hospital": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///     }
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDonation(Guid id, DonationUpdateDTO donationUpdateDto)
        {
            try
            {
                var result = await _donationService.UpdateDonation(id, donationUpdateDto);
                return result ? Ok(new { message = "Doação atualizada com sucesso!" }) : BadRequest(new { message = "Erro ao atualizar doação!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = $"Erro: {e.Message}" });
            }
        }

        /// <summary>
        /// Realiza e exclusão de uma doação do sistema
        /// </summary>
        /// <param name="id">ID da doação que será excluída</param>
        /// <returns>exclui uma doação correspondente ao id que foi informado</returns>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     DELETE api/Donation/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonation(Guid id)
        {
            var result = await _donationService.DeleteDonation(id);
            if (result)
            {
                return Ok(new { message = "Doação removida com sucesso!" });
            }

            return BadRequest(new { message = "Erro ao remover doação!" });
        }
    }
}
