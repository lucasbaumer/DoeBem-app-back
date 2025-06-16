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

        public async Task<IEnumerable<DonorWithDonationsDTO>> GetAllWithDonationAsync()
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

        public async Task<IEnumerable<DonorDTO>> GetAllDonorsAsync()
        {
            var donors = await _donorRepository.GetAllAsync();

            return donors.Select(d => new DonorDTO
            {
                Id = d.Id,
                Name = d.Name,
                Cpf = d.Cpf,
                Phone = d.Phone,
                Email = d.Email,
                DateOfBirth = d.DateOfBirth.ToString("yyyy-MM-dd")
            });
        }

        public async Task<DonorWithDonationsDTO> GetByIdWithDonationAsync(Guid id)
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

        public async Task<DonorDTO> GetDonorByIdAsync(Guid id)
        {
            var donor = await _donorRepository.GetByIdAsync(id);

            if (donor == null)
            {
                return null;
            }

            return new DonorDTO
            {
                Id = donor.Id,
                Name = donor.Name,
                Cpf = donor.Cpf,
                Phone = donor.Phone,
                Email = donor.Email,
                DateOfBirth = donor.DateOfBirth.ToString("yyyy-MM-dd")
            };
        }

        public async Task<Guid> RegisterDonor(DonorCreateDTO donorCreateDto)
        {
            var usuarioExistente = await _userManager.FindByEmailAsync(donorCreateDto.Email);

            if (usuarioExistente != null)
            {
                throw new Exception("Email já cadastrado");
            }

            var Cpfexistente = await _donorRepository.GetByCpfAsync(donorCreateDto.Cpf);
            if (Cpfexistente != null)
            {
                throw new Exception("Cpf já cadastrado!");
            }

            var usuario = new IdentityUser
            {
                UserName = donorCreateDto.Email,
                Email = donorCreateDto.Email,
                PhoneNumber = donorCreateDto.Phone
            };

            var result = await _userManager.CreateAsync(usuario, donorCreateDto.Password);
            await _userManager.AddToRoleAsync(usuario, "donor");

            if (!result.Succeeded)
            {
                var erros = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Erro ao criar usuário: {erros}");
            }

            try
            {
                if (!DateTime.TryParseExact(donorCreateDto.DateOfBirth, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime dateOfBirth))
                {
                    throw new Exception("Data de nascimento inválida, formato esperado yyyy-MM-dd");
                }

                var donor = new Donor
                {
                    Id = Guid.NewGuid(),
                    Name = donorCreateDto.Name,
                    Email = donorCreateDto.Email,
                    Phone = donorCreateDto.Phone,
                    Cpf = donorCreateDto.Cpf,
                    DateOfBirth = dateOfBirth
                };

                await _donorRepository.AddAsync(donor);
                return donor.Id;
            }
            catch (Exception err)
            {
                await _userManager.DeleteAsync(usuario);
                throw new Exception("Erro ao cadastrar doador!!" + err.Message);
            }
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

        public async Task<DonorDTO> GetDonorByEmailAsync(string email)
        {
            var donor = await _donorRepository.GetByEmailAsync(email);
            if(donor == null)
            {
                return null;
            }

            return new DonorDTO
            {
                Id = donor.Id,
                Name = donor.Name,
                Cpf = donor.Cpf,
                Phone = donor.Phone,
                Email = donor.Email,
                DateOfBirth = donor.DateOfBirth.ToString("yyyy-MM-dd")
            };
        }
    }
}
