using doeBem.Application.DTOS;
using doeBem.Application.Interfaces;
using doeBem.Core.Entities;
using doeBem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Application.Services
{
    public class HospitalService : IHospitalService
    {
        private readonly IHospitalRepository _hospitalRepository;

        public HospitalService(IHospitalRepository hospitalRepository)
        {
            _hospitalRepository = hospitalRepository;
        }

        public async Task<bool> DeleteHospital(Guid id)
        {
            await _hospitalRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<Hospital>> GetAllAsync()
        {
            return await _hospitalRepository.GetAllAsync();
        }

        public async Task<Hospital> GetByIdAsync(Guid id)
        {
            return await _hospitalRepository.GetByIdAsync(id);
        }

        public async Task<Guid> RegisterHospital(HospitalCreateDto registerHospitalDto)
        {
            if(registerHospitalDto.CNES < 1000000 || registerHospitalDto.CNES > 9999999)
            {
                throw new Exception("CNES está inválido. Deve conter exatamente 7 dígitos");
            }

            var hospital = new Hospital
            {
                Id = Guid.NewGuid(),
                Name = registerHospitalDto.Name,
                CNES = registerHospitalDto.CNES,
                State = registerHospitalDto.State,
                City = registerHospitalDto.City,
                Phone = registerHospitalDto.Phone,
                Description = registerHospitalDto.Description
            };

            await _hospitalRepository.AddAsync(hospital);
            return hospital.Id;
        }

        public async Task<bool> UpdateHospital(Guid id, HospitalUpdateDto updateHospitalDto)
        {
            var hospital = await _hospitalRepository.GetByIdAsync(id);
            if (hospital == null)
            {
                throw new Exception("Hospital não encontrado");
            }

            hospital.Name = updateHospitalDto.Name;
            hospital.Phone = updateHospitalDto.Phone;
            hospital.CNES = updateHospitalDto.CNES;
            hospital.State = updateHospitalDto.State;
            hospital.City = updateHospitalDto.City;
            hospital.Description = updateHospitalDto.Description;

            await _hospitalRepository.UpdateAsync(hospital);
            return true;
        }
    }
}
