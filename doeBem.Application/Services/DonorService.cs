using doeBem.Application.DTOS;
using doeBem.Application.Interfaces;
using doeBem.Core.Entities;
using doeBem.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace doeBem.Application.Services
{
    public class DonorService : IDonorService
    {
        private readonly IDonorRepository _donorRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public DonorService(IDonorRepository donorRepository, UserManager<IdentityUser> userManager)
        {
            _donorRepository = donorRepository;
            _userManager = userManager;
        }

        public async Task<bool> DeleteDonor(Guid id)
        {
            var donor = await _donorRepository.GetByIdAsync(id);
            if(donor == null)
            {
                throw new Exception("Doador não foi encontrado");
            }

            var user = await _userManager.FindByEmailAsync(donor.Email);
            if(user != null)
            {
                var identityResult = await _userManager.DeleteAsync(user);
                if (!identityResult.Succeeded)
                {
                    var erros = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                    throw new Exception($"Erro ao deletar usuário: ${erros}");
                }
            }

            await _donorRepository.DeleteAsync(id);
            return true; 
        }

        public async Task<IEnumerable<DonorWithDonationsDTO>> GetAllAsync()
        {
            var donors = await _donorRepository.GetAllAsync();

            return donors.Select(d => new DonorWithDonationsDTO
            {
                Id = d.Id,
                Name = d.Name,
                Cpf = d.Cpf,
                Phone = d.Phone,
                Email = d.Email,
                DateOfBirth = d.DateOfBirth.ToString("yyyy-MM-dd"),
                Donations = d.Donations.Select(dn => new DonationForDonorDTO
                {
                    Id = dn.Id,
                    Value = dn.Value,
                    Date = dn.Date,
                    HospitalId = dn.HospitalId ?? Guid.Empty,
                    HospitalName = dn.Hospital?.Name
                }).ToList()
            });
        }

        public async Task<DonorWithDonationsDTO> GetByIdAsync(Guid id)
        {
            var donor = await _donorRepository.GetByIdAsync(id);

            if(donor == null)
            {
                return null;
            }

            return new DonorWithDonationsDTO
            {
                Id = donor.Id,
                Name = donor.Name,
                Cpf = donor.Cpf,
                Phone = donor.Phone,
                Email = donor.Email,
                DateOfBirth = donor.DateOfBirth.ToString("yyyy-MM-dd"),
                Donations = donor.Donations.Select(dn => new DonationForDonorDTO
                {
                    Id = dn.Id,
                    Value = dn.Value,
                    Date = dn.Date,
                    HospitalId = dn.HospitalId ?? Guid.Empty,
                    HospitalName = dn.Hospital?.Name
                }).ToList()
            }; 
        }

        public async Task<Guid> RegisterDonor(DonorCreateDTO registerDonorDto)
        {
            if (!DateTime.TryParseExact(registerDonorDto.DateOfBirth, "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime dateOfBirth))
            {
                throw new Exception("Data de nascimento inválidam formato esperado yyyy-MM-dd");
            }

            var donor = new Donor
            {
                Id = Guid.NewGuid(),
                Name = registerDonorDto.Name,
                Email = registerDonorDto.Email,
                Cpf = registerDonorDto.Cpf,
                Phone = registerDonorDto.Phone,
                DateOfBirth = dateOfBirth
            };

            await _donorRepository.AddAsync(donor);
            return donor.Id;
        }

        public async Task<bool> UpdateDonor(Guid id, DonorUpdateDTO updateDto)
        {
            if (!DateTime.TryParseExact(updateDto.DateOfBirth, "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime dateOfBirth))
            {
                throw new Exception("Data de nascimento inválida, formato esperado yyyy-MM-dd");
            }

            var donor = await _donorRepository.GetByIdAsync(id);
            if (donor == null)
            {
                throw new Exception("Doador não encontrado!");
            }

            donor.Name = updateDto.Name;
            donor.Email = updateDto.Email;
            donor.Phone = updateDto.Phone;
            donor.Cpf = updateDto.Cpf;
            donor.DateOfBirth = dateOfBirth;

            await _donorRepository.UpdateAsync(donor);
            return true;
        }
    }
}
