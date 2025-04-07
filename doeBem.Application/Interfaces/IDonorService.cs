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
        Task<Donor> GetByIdAsync(Guid id);
        Task<IEnumerable<Donor>> GetAllAsync();
        Task<Guid> RegisterDonor(DonorCreateDTO CreateDonorDto);
        Task<bool> UpdateDonor(Guid id, DonorUpdateDTO updateDonorDto);
        Task<bool> DeleteDonor(Guid id);
        Task<bool> LoginAsync(string email, string passoword);
    }
}
