using doeBem.Application.DTOS;
using doeBem.Application.Interfaces;
using doeBem.Core.Entities;
using doeBem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Application.Services
{
    public class DonationService : IDonationService
    {
        private readonly IDonationRepository _donationRepository;

        public DonationService(IDonationRepository donationRepository)
        {
            _donationRepository = donationRepository;
        }

        public async Task<Guid> RegisterDonation(DonationCreateDTO donationCreateDto)
        {
            if (!DateTime.TryParse(donationCreateDto.Date, out DateTime Date))
            {
                throw new Exception("Data de nascimento inválida");
            }

            var donation = new Donation
            {
                Id = Guid.NewGuid(),
                Value = donationCreateDto.Value,
                Date = Date,
                DonorId = donationCreateDto.DonorId,
                HospitalId = donationCreateDto.HospitalId
            };

            await _donationRepository.AddAsync(donation);
            return donation.Id;
        }

        public async Task<bool> DeleteDonation(Guid id)
        {
            await _donationRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<DonationDTO>> GetAllAsync()
        {
            var donations = await _donationRepository.GetAllAsync();

            return donations.Select(donation => new DonationDTO
            {
                Id = donation.Id,
                Value = donation.Value,
                Date = donation.Date.ToString("yyyy-MM-dd"),
                DonorId = donation.DonorId,
                DonorName = donation.Donor.Name,
                HospitalId = donation.HospitalId,
                HospitalName = donation.Hospital.Name
            });
        }

        public async Task<DonationDTO> GetByIdAsync(Guid id)
        {
            var donation = await _donationRepository.GetByIdAsync(id);
            if (donation == null)
            {
                throw new Exception("Nenhuma doação encontrada!");
            }

            return new DonationDTO
            {
                Id = donation.Id,
                Value = donation.Value,
                Date = donation.Date.ToString("yyyy-MM-dd"),
                DonorId = donation.DonorId,
                DonorName = donation.Donor.Name,
                HospitalId = donation.HospitalId,
                HospitalName = donation.Hospital.Name
            };
        }

        public async Task<bool> UpdateDonation(Guid id, DonationDTO donationDto)
        {
            var donation = await _donationRepository.GetByIdAsync(id);
            if(donation == null)
            {
                throw new Exception("Doação não encontrada!");
            }

            if (!DateTime.TryParse(donationDto.Date, out DateTime Date))
            {
                throw new Exception("Data de nascimento inválida");
            }

            donation.Value = donationDto.Value;
            donation.Date = Date;
            donation.DonorId = donationDto.DonorId;
            donation.HospitalId = donation.HospitalId;

            await _donationRepository.UpdateAsync(donation);
            return true;
        }
    }
}
