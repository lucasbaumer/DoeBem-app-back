using doeBem.Application.DTOS;
using doeBem.Application.Interfaces;
using doeBem.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace doeBem.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HospitalController : ControllerBase
    {
        private readonly IHospitalService _hospitalService;

        public HospitalController(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;
        }

        [HttpGet]
        public async Task<ActionResult<Hospital>> GetAllHospitals()
        {

            var hospitals =  await _hospitalService.GetAllAsync();

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

        [HttpGet("id")]
        public async Task<ActionResult<HospitalWithDonationsDto>> GetHospitalById(Guid id)
        {
            var hospital = await _hospitalService.GetByIdAsync(id);
            if(hospital == null)
            {
                return NotFound("Nenhum hospital com o id foi encontrado");
            }

            return Ok(hospital);
        }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHospital(Guid id)
        {
            var result = await _hospitalService.DeleteHospital(id);
            if (result == null)
            {
                return BadRequest("Erro ao remover hospital!");
            }

            return Ok("Hospital removido com sucesso!");
        }
    }
}
