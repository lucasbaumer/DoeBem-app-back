using doeBem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Core.Interfaces
{
    public interface IDonationRepository
    {
        Task<IEnumerable<Donation>> GetAllAsync();
        Task<Donation> GetByIdAsync(Guid id);
        Task AddAsync(Donation donation);
        Task UpdateAsync(Donation donation);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Donation>> GetByHospitalIdAsync(Guid hospitalId);
    }
}
