﻿using doeBem.Application.DTOS;
using doeBem.Application.Interfaces;
using doeBem.Core;
using doeBem.Core.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace doeBem.Application.Services
{
    public class DonorService : IDonorService
    {
        private readonly IDonorRepository _donorRepository;
        private readonly EncryptService _encryptService;

        public DonorService(IDonorRepository donorRepository)
        {
            _donorRepository = donorRepository;
            _encryptService = new EncryptService("chaveSecreta");
        }

        public async Task<bool> DeleteDonor(Guid id)
        {
            await _donorRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<Donor>> GetAllAsync()
        {
            return await _donorRepository.GetAllAsync();
        }

        public async Task<Donor> GetByIdAsync(Guid id)
        {
            return await _donorRepository.GetByIdAsync(id);
        }

        public async Task<Guid> RegisterDonor(DonorCreateDTO dto)
        {
            if (!DateTime.TryParse(dto.DateOfBirth, out DateTime dateOfBirth))
            {
                throw new Exception("Data de nascimento inválida");
            }

            string encryptedPassoword = _encryptService.Encrypt(dto.Password);

            var donor = new Donor
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Cpf = dto.Cpf,
                Phone = dto.Phone,
                DateOfBirth = dateOfBirth,
                PasswordCript = encryptedPassoword
            };

            await _donorRepository.AddAsync(donor);
            return donor.Id;
        }

        public async Task<bool> UpdateDonor(Guid id, DonorUpdateDTO updateDto)
        {
            var donor = await _donorRepository.GetByIdAsync(id);
            if (donor == null)
            {
                throw new Exception("Doador não encontrado!");
            }

            donor.Name = updateDto.Name;
            donor.Email = updateDto.Email;
            donor.Phone = updateDto.Phone;
            donor.Cpf = updateDto.Cpf;
            donor.DateOfBirth = updateDto.DateOfBirth;

            await _donorRepository.UpdateAsync(donor);
            return true;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var donor = await _donorRepository.GetByEmailAsync(email);
            if(donor == null)
            {
                return false;
            }

            string decryptedPassword = _encryptService.Decrypt(donor.PasswordCript);
            return decryptedPassword == password;
        }

        public async Task<bool> ValidatePassword(Guid id, string password)
        {
            var donor = await _donorRepository.GetByIdAsync(id);
            if(donor == null)
            {
                return false;
            }

            string decryptedPassword = _encryptService.Decrypt(donor.PasswordCript);

            return decryptedPassword == password;
        }
    }
}
