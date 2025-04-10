﻿using doeBem.Application.DTOS;
using doeBem.Application.Interfaces;
using doeBem.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace doeBem.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonorController : ControllerBase
    {
        private readonly IDonorService _donorService;

        public DonorController(IDonorService donorService)
        {
            _donorService = donorService;
        }

        [HttpGet]
        public async Task<IEnumerable<Donor>> GetAllDonors()
        {
            return await _donorService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Donor>> GetDonorById(Guid id)
        {
            var donor = await _donorService.GetByIdAsync(id);
            if (donor == null)
            {
                return NotFound();
            }

            return Ok(donor);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterDonor(DonorCreateDTO donorCreate)
        {
            try
            {
                var donorId = await _donorService.RegisterDonor(donorCreate);
                return CreatedAtAction(nameof(GetDonorById), new { id = donorId }, donorCreate);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cadastrar doador: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDonor(Guid id, DonorUpdateDTO updateDto)
        {
            try
            {
                var result = await _donorService.UpdateDonor(id, updateDto);
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


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            bool isValid = await _donorService.LoginAsync(loginDto.Email, loginDto.Password);

            if (isValid)
            {
                return Ok("Login bem sucedido!");
            }

            return Unauthorized("Email ou senha inválidos!");
        }
    }
}
