using doeBem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Core.Interfaces
{
    public interface IDonorRepository
    {
        Task AddAsync(Donor donor);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(Donor donor);
        Task<Donor> GetByIdAsync(Guid id);
        Task<Donor> GetByEmailAsync(string email);
        Task<Donor?> GetByCpfAsync(string cpf);
        Task<IEnumerable<Donor>> GetAllAsync();
    }
}
