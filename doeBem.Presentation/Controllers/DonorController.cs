using doeBem.Application.DTOS;
using doeBem.Application.Interfaces;
using doeBem.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace doeBem.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DonorController : ControllerBase
    {
        private readonly IDonorService _donorService;

        public DonorController(IDonorService donorService)
        {
            _donorService = donorService;
        }

        /// <summary>
        /// Realiza o retorno de todas os doadores registrados no sistema
        /// </summary>
        /// <returns>Retorna todos os doadores e suas doações</returns>
        /// <remarks>
        /// Exemplo de request: 
        /// 
        ///         GET api/Donor
        ///         
        /// Exemplo de resposta:
        ///     
        ///     [
        ///         {
        ///             "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "nome": "Carlos",
        ///             "email": "Carlos123@gmail.com",
        ///             "cpf": "999.999.999-99",
        ///             "phone": "(41)99999-9999",
        ///             "dataNascimento": "1980-04-20",
        ///             "doacoes": [
        ///                 {
        ///                     "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///                     "valor": 1000.00,
        ///                     "data": "2025-04-20",
        ///                     "idDoador": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///                     "nomeDoador": "Carlos",
        ///                     "idHospital": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///                     "nomeHospital": "Hospital IPO"
        ///                 }
        ///             ]
        ///         },
        ///         {
        ///             "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "nome": "Pedro",
        ///             "email": "Pedro123@gmail.com",
        ///             "cpf": "999.999.999-99",
        ///             "phone": "(41)99999-9999",
        ///             "dataNascimento": "1980-04-20",
        ///             "doacoes": [
        ///                 {
        ///                     "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///                     "valor": 1200.00,
        ///                     "data": "2025-04-20",
        ///                     "idDoador": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///                     "nomeDoador": "Pedro",
        ///                     "idHospital": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///                     "nomeHospital": "Hospital Pequeno Príncipe"
        ///                 }
        ///             ]
        ///         }
        ///     ]
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Donor>>> GetAllDonors()
        {
            var donors = await _donorService.GetAllAsync();

            if(donors == null)
            {
                return NotFound("Nenhum Doador encontrado");
            }

            if (!donors.Any())
            {
                return NotFound("A lista de doadores está vazia");
            }

            return Ok(donors);
        }

        /// <summary>
        /// Realiza o retorno de um doador com o id passado
        /// </summary>
        /// <param name="id">ID do doador que será retornada</param>
        /// <returns>Retorna o doador pelo id que foi informado com suas doações</returns>
        /// <remarks>
        /// Exemplo de request: 
        /// 
        ///     GET api/Donor/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     
        /// Exemplo de resposta:
        /// 
        ///         {
        ///             "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "nome": "Carlos",
        ///             "email": "Carlos123@gmail.com",
        ///             "cpf": "999.999.999-99",
        ///             "phone": "(41)99999-9999",
        ///             "dataNascimento": "1980-04-20",
        ///             "doacoes": [
        ///                 {
        ///                     "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///                     "valor": 1000.00,
        ///                     "data": "2025-04-20",
        ///                     "idDoador": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///                     "nomeDoador": "Carlos",
        ///                     "idHospital": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///                     "nomeHospital": "Hospital IPO"
        ///                 }
        ///             ]
        ///         }
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Donor>> GetDonorById(Guid id)
        {
            var donor = await _donorService.GetByIdAsync(id);
            if (donor == null)
            {
                return NotFound("Nenhum doador foi encontrado");
            }

            return Ok(donor);
        }

        /// <summary>
        /// Realiza a edição de um doador
        /// </summary>
        /// <param name="id">ID do doador que será atualizado</param>
        /// <returns>Edita o doador pelo id que foi informado </returns>
        /// <remarks>
        /// Exemplo de request: 
        /// 
        ///     PUT api/Donor/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     
        /// Exemplo de resposta:
        /// 
        ///       {
        ///          "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///          "nome": "Carlos",
        ///          "email": "Carlos123@gmail.com",
        ///          "cpf": "999.999.999-99",
        ///          "phone": "(41)99999-9999",
        ///          "dataNascimento": "1980-04-20",
        ///       }
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDonor(Guid id, DonorUpdateDTO DonorUpdateDto)
        {
            try
            {
                var result = await _donorService.UpdateDonor(id, DonorUpdateDto);
                return result ? Ok("Doador atualizado com sucesso!") : BadRequest("Erro ao atualizar doador!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Realiza a exlusão de um doador do sistema
        /// </summary>
        /// <param name="id">ID do doador que será excluída</param>
        /// <returns>exclui um doador correspondente ao id que foi informado</returns>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     DELETE api/Donor/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonor(Guid id)
            
        {
            var result = await _donorService.DeleteDonor(id);
            if (result)
            {
                return Ok("Doador removido com sucesso!");
            }
            return BadRequest("Erro ao remover doador!");
        }
    }
}
