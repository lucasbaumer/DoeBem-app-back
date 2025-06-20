using doeBem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Core.Interfaces
{
    public interface IHospitalRepository
    {
        Task AddAsync(Hospital hospital);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(Hospital hospital);
        Task<Hospital> GetByIdAsync(Guid id);
        Task<Hospital> GetByCnesAsync(int cnes);
        Task<IEnumerable<Hospital>> GetAllAsync();
    }
}
