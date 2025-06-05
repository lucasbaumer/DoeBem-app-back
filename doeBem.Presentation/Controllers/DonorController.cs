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

        [HttpGet]
        public async Task<ActionResult<Donor>> GetAllDonors()
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

        [HttpPost]
        public async Task<IActionResult> RegisterDonor(DonorCreateDTO donorCreateDto)
        {
            try
            {
                var donorId = await _donorService.RegisterDonor(donorCreateDto);
                return CreatedAtAction(nameof(GetDonorById), new { id = donorId }, donorCreateDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cadastrar doador: {ex.Message}");
            }
        }

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
