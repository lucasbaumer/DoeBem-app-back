using doeBem.Application.DTOS;
using doeBem.Application.Interfaces;
using doeBem.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace doeBem.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonationController : ControllerBase
    {
        private readonly IDonationService _donationService;
        public DonationController(IDonationService donationService)
        {
            _donationService = donationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonationDTO>>> GetAllDonations()
        {
            var donations = await _donationService.GetAllAsync();

            if (donations == null)
            {
                return NotFound("Nenhuma doação encontrada!");
            }

            if (!donations.Any())
            {
                return NotFound("A lista de doações está vazia");
            }

            return Ok(donations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Donation>> GetDonationById(Guid id)
        {
            var donation = await _donationService.GetByIdAsync(id);
            if (donation == null)
            {
                return NotFound("Nenhuma doação foi encontrada");
            }

            return Ok(donation);
        }

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
                return BadRequest($"Erro ao cadastrar doação: {e.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDonation(Guid id, DonationDTO donationDto)
        {
            try
            {
                var result = await _donationService.UpdateDonation(id, donationDto);
                return result ? Ok("Doação atualizada com sucesso!") : BadRequest("Erro ao atualizar doação!");
            }
            catch (Exception e)
            {
                return BadRequest($"Erro: {e.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonation(Guid id)
        {
            var result = await _donationService.DeleteDonation(id);
            if (result)
            {
                return Ok("Doação removida com sucesso!");
            }

            return BadRequest("Erro ao remover doação!");
        }
    }
}
