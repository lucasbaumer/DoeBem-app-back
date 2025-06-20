using doeBem.Application.DTOS;
using doeBem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Application.Interfaces
{
    public interface IDonorService
    {
        Task<DonorWithDonationsDTO> GetByIdWithDonationAsync(Guid id);
        Task<DonorDTO> GetDonorByIdAsync(Guid id);
        Task<DonorDTO> GetDonorByEmailAsync(string email);
        Task<IEnumerable<DonorWithDonationsDTO>> GetAllWithDonationAsync();
        Task<IEnumerable<DonorDTO>> GetAllDonorsAsync();
        Task<Guid> RegisterDonor(DonorCreateDTO CreateDonorDto);
        Task<bool> UpdateDonor(Guid id, DonorUpdateDTO updateDonorDto);
        Task<bool> DeleteDonor(Guid id);
    }
}
