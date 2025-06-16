using doeBem.Application.DTOS;
using doeBem.Application.Interfaces;
using doeBem.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace doeBem.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HospitalController : ControllerBase
    {
        private readonly IHospitalService _hospitalService;

        public HospitalController(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;
        }

        /// <summary>
        /// Realiza o retorno de todos os hospitais com as doações recebidas registrados no sistema
        /// </summary>
        /// <returns>Retorna todos os Hospitais e as doações recebidas</returns>
        /// <remarks>
        /// Exemplo de request: 
        /// 
        ///         GET api/Hospital/WithDonations
        ///         
        /// Exemplo de resposta:
        ///     
        ///     [
        ///         {
        ///             "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "nome": "Hospital Pequeno Príncipe",
        ///             "cnes": "1234567",
        ///             "estado": "Paraná",
        ///             "cidade": "Curitiba",
        ///             "phone": "(41)99999-9999",
        ///             "descrição": "Descrição"
        ///                 "Doações recebidas": 
        ///                 [
        ///                      {
        ///                         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///                         "value": 1050.9,
        ///                         "date": "2025-01-20",
        ///                         "donorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///                         "donorName": "Carlos"
        ///                       },
        ///                 ]
        ///         },
        ///         {
        ///             "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "nome": "Hospital IPO",
        ///             "cnes": "1234567",
        ///             "estado": "Paraná",
        ///             "cidade": "Curitiba",
        ///             "phone": "(41)99999-9999",
        ///             "descrição": "Descrição"
        ///                 "Doações recebidas": 
        ///                 [
        ///                      {
        ///                         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///                         "value": 1150.9,
        ///                         "date": "2025-01-20",
        ///                         "donorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///                         "donorName": "Pedro"
        ///                       },
        ///                 ]
        ///         }
        ///     ]
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("WithDonations")]
        public async Task<ActionResult<Hospital>> GetAllHospitalsWithDonations()
        {

            var hospitals =  await _hospitalService.GetAllWithDonationsAsync();

            if(hospitals == null)
            {
                return NotFound("Nenhum Hospital encontrado!");
            }
            if (!hospitals.Any())
            {
                return NotFound("A lista de Hospitais está vazia!");
            }

            return Ok(hospitals);
        }

        /// <summary>
        /// Realiza o retorno de todos os hospitais sem doações recebidas registrados no sistema
        /// </summary>
        /// <returns>Retorna todos os Hospitais</returns>
        /// <remarks>
        /// Exemplo de request: 
        /// 
        ///         GET api/Hospital
        ///         
        /// Exemplo de resposta:
        ///     
        ///     [
        ///         {
        ///             "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "nome": "Hospital Pequeno Príncipe",
        ///             "cnes": "1234567",
        ///             "estado": "Paraná",
        ///             "cidade": "Curitiba",
        ///             "phone": "(41)99999-9999",
        ///             "descrição": "Descrição"
        ///         },
        ///         {
        ///             "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "nome": "Hospital IPO",
        ///             "cnes": "1234567",
        ///             "estado": "Paraná",
        ///             "cidade": "Curitiba",
        ///             "phone": "(41)99999-9999",
        ///             "descrição": "Descrição"
        ///         }
        ///     ]
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<HospitalDTO>> GetAllHospitals()
        {

            var hospitals = await _hospitalService.GetAllHospitalsAsync();

            if (hospitals == null)
            {
                return NotFound("Nenhum Hospital encontrado!");
            }
            if (!hospitals.Any())
            {
                return NotFound("A lista de Hospitais está vazia!");
            }

            return Ok(hospitals);
        }

        /// <summary>
        /// Realiza o retorno de um hospital e as doações recebidas com a id passada
        /// </summary>
        /// <param name="id">ID do Hospital que será retornada</param>
        /// <returns>Retorna o Hospital pelo id que foi informado e as doações recebidas</returns>
        /// <remarks>
        /// Exemplo de request: 
        /// 
        ///     GET api/Hospital/WithDonations/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     
        /// Exemplo de resposta:
        /// 
        ///         {
        ///             "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "nome": "Hospital IPO",
        ///             "cnes": "1234567",
        ///             "estado": "Paraná",
        ///             "cidade": "Curitiba",
        ///             "phone": "(41)99999-9999",
        ///             "descrição": "Descrição"
        ///                 "Doações recebidas": [
        ///                      {
        ///                         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///                         "value": 1050.9,
        ///                         "date": "2025-01-20",
        ///                         "donorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///                         "donorName": "Carlos"
        ///                       },
        ///         }
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("WithDonations/{id}")]
        public async Task<ActionResult<HospitalWithDonationsDto>> GetHospitalWithDonationsById(Guid id)
        {
            var hospital = await _hospitalService.GetByIdWithDonationsAsync(id);
            if(hospital == null)
            {
                return NotFound("Nenhum hospital com o id foi encontrado");
            }

            return Ok(hospital);
        }

        /// <summary>
        /// Realiza o retorno de um hospital sem doações recebidas com a id passada
        /// </summary>
        /// <param name="id">ID do Hospital que será retornada</param>
        /// <returns>Retorna o Hospital pelo id que foi informado</returns>
        /// <remarks>
        /// Exemplo de request: 
        /// 
        ///     GET api/Hospital/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     
        /// Exemplo de resposta:
        /// 
        ///         {
        ///             "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "nome": "Hospital IPO",
        ///             "cnes": "1234567",
        ///             "estado": "Paraná",
        ///             "cidade": "Curitiba",
        ///             "phone": "(41)99999-9999",
        ///             "descrição": "Descrição"
        ///         }
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<HospitalDTO>> GetHospitalById(Guid id)
        {
            var hospital = await _hospitalService.GetHospitalByIdAsync(id);
            if (hospital == null)
            {
                return NotFound("Nenhum hospital com o id foi encontrado");
            }

            return Ok(hospital);
        }

        /// <summary>
        /// Realiza o cadastro de um hospital no sistema
        /// </summary>
        /// <param name="hospitalCreateDto">Objeto contendo os dados do Hospital</param>
        /// <returns>Cadastra o hospital no banco de dados e retorna mensagem de sucesso</returns>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     POST api/Hospital
        ///     
        /// Exemplo de resposta:
        /// 
        ///       {
        ///          "nome": "Hospital IPO",
        ///          "cnes": "1234567",
        ///          "estado": "Paraná",
        ///          "cidade": "Curitiba",
        ///          "phone": "(41)99999-9999",
        ///          "descrição": "Descrição"
        ///       }
        ///     
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> RegisterHospital(HospitalCreateDto hospitalCreateDto)
        {
            try
            {
                var HospitalId = await _hospitalService.RegisterHospital(hospitalCreateDto);
                return CreatedAtAction(nameof(GetHospitalById), new { id = HospitalId }, hospitalCreateDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cadastrar novo hospital: {ex.Message}");
            }
        }

        /// <summary>
        /// Realiza a edição de um hospital
        /// </summary>
        /// <param name="id">ID do hospital que será atualizado</param>
        /// <returns>Edita o doador pelo id que foi informado </returns>
        /// <remarks>
        /// Exemplo de request: 
        /// 
        ///     PUT api/Hospital/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     
        /// Exemplo de resposta:
        /// 
        ///       {
        ///          "nome": "Hospital IPO",
        ///          "cnes": "1234567",
        ///          "estado": "Paraná",
        ///          "cidade": "Curitiba",
        ///          "phone": "(41)99999-9999",
        ///          "descrição": "Descrição"
        ///       }
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHospital(Guid id, HospitalUpdateDto hospitalUpdateDto)
        {
            try
            {
                var result = await _hospitalService.UpdateHospital(id, hospitalUpdateDto);
                return result ? Ok("Hospital atualizado com sucesso!") : BadRequest("Erro ao atualizar Hospital!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Realiza a exclusão de um hospital do sistema
        /// </summary>
        /// <summary>
        /// Realiza a exlusão de um hospital do sistema
        /// </summary>
        /// <param name="id">ID do hospital que será excluído</param>
        /// <returns>exclui um hospital correspondente ao id que foi informado</returns>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     DELETE api/Hospital/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHospital(Guid id)
        {
            var result = await _hospitalService.DeleteHospital(id);
            if (!result)
            {
                return BadRequest("Erro ao remover hospital!");
            }

            return Ok("Hospital removido com sucesso!");
        }
    }
}
