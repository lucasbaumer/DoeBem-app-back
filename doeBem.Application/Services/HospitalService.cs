﻿using doeBem.Application.DTOS;
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
        private readonly IDonationRepository _donationRepository;

        public HospitalService(IHospitalRepository hospitalRepository, IDonationRepository donationRepository)
        {
            _hospitalRepository = hospitalRepository;
            _donationRepository = donationRepository;
        }

        public async Task<bool> DeleteHospital(Guid id)
        {
            var hospital = await _hospitalRepository.GetByIdAsync(id);
            if(hospital == null)
            {
                throw new Exception("Hospital não foi encontrado!");
            }

            var donations = await _donationRepository.GetByHospitalIdAsync(hospital.Id);
            foreach(var donation in donations)
            {
                donation.HospitalId = null;
                await _donationRepository.UpdateAsync(donation);
            }

            await _hospitalRepository.DeleteAsync(hospital.Id);
            return true;
        }

        public async Task<IEnumerable<HospitalDTO>> GetAllHospitalsAsync()
        {
            var hospitals = await _hospitalRepository.GetAllAsync();

            return hospitals.Select(h => new HospitalDTO
            {
                Id = h.Id,
                Name = h.Name,
                CNES = h.CNES,
                State = h.State,
                City = h.City,
                Phone = h.Phone,
                Description = h.Description
            }).ToList();
        }

        public async Task<IEnumerable<HospitalWithDonationsDto>> GetAllWithDonationsAsync()
        {
            var hospitals = await _hospitalRepository.GetAllAsync();

            return hospitals.Select(h => new HospitalWithDonationsDto
            {
                Id = h.Id,
                Name = h.Name,
                CNES = h.CNES,
                State = h.State,
                City = h.City,
                Phone = h.Phone,
                Description = h.Description,
                ReceivedDonations = h.ReceivedDonations.Select(d => new DonationForHospitalDTO
                {
                    Id = d.Id,
                    Value = d.Value,
                    Date = d.Date,
                    DonorId = d.DonorId ?? Guid.Empty,
                    DonorName = d.Donor?.Name ?? "Doador Anônimo"
                }).ToList()
            }).ToList();
        }

        public async Task<HospitalWithDonationsDto> GetByIdWithDonationsAsync(Guid id)
        {
            var hospital = await _hospitalRepository.GetByIdAsync(id);

            if (hospital == null)
            {
                throw new Exception("Hospital com o id selecinado nao existe!");
            }

            return new HospitalWithDonationsDto
            {
                Id = hospital.Id,
                Name = hospital.Name,
                CNES = hospital.CNES,
                State = hospital.State,
                City = hospital.City,
                Phone = hospital.Phone,
                Description = hospital.Description,
                ReceivedDonations = hospital.ReceivedDonations.Select(d => new DonationForHospitalDTO
                {
                    Id = d.Id,
                    Value = d.Value,
                    Date = d.Date,
                    DonorId = d.DonorId ?? Guid.Empty,
                    DonorName= d.Donor?.Name ?? "Doador Anônimo"

                }).ToList()


            };
        }

        public async Task<HospitalDTO> GetHospitalByIdAsync(Guid id)
        {
            var hospital = await _hospitalRepository.GetByIdAsync(id);

            if (hospital == null)
            {
                throw new Exception("Hospital com o id selecinado nao existe!");
            }

            return new HospitalDTO
            {
                Id = hospital.Id,
                Name = hospital.Name,
                CNES = hospital.CNES,
                State = hospital.State,
                City = hospital.City,
                Phone = hospital.Phone,
                Description = hospital.Description
            };
        }

        public async Task<Guid> RegisterHospital(HospitalCreateDto registerHospitalDto)
        {
            if(registerHospitalDto.CNES < 1000000 || registerHospitalDto.CNES > 9999999)
            {
                throw new Exception("CNES está inválido. Deve conter exatamente 7 dígitos");
            }

            var cnesExitente = await _hospitalRepository.GetByCnesAsync(registerHospitalDto.CNES);
            if(cnesExitente != null)
            {
                throw new Exception("CNES já cadastrado!");
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
