using doeBem.Application.DTOS;
using doeBem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Application.Interfaces
{
    public interface IDonationService
    {
        Task<IEnumerable<DonationDTO>> GetAllAsync();
        Task<DonationDTO> GetByIdAsync(Guid id);
        Task<Guid> RegisterDonation(DonationDTO donationDto);
        Task<bool> UpdateDonation(Guid id, DonationDTO donationDto);
        Task<bool> DeleteDonation(Guid id);

    }
}
