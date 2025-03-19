using doeBem.Application.DTOS;
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

        public DonorService(IDonorRepository donorRepository)
        {
            _donorRepository = donorRepository;
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

            string hashedPassword = HashPassword(dto.Password);

            var donor = new Donor
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Cpf = dto.Cpf,
                Phone = dto.Phone,
                DateOfBirth = dateOfBirth,
                PasswordHash = hashedPassword
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

        private string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
