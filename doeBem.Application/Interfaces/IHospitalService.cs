using doeBem.Application.DTOS;
using doeBem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Application.Interfaces
{
    public interface IHospitalService
    {
        Task<IEnumerable<Hospital>> GetAllAsync();
        Task<Hospital> GetByIdAsync(Guid id);
        Task<Guid> RegisterHospital(HospitalCreateDto registerHospitalDto);
        Task<bool> UpdateHospital(Guid id, HospitalUpdateDto updateHospitalDto);
        Task<bool> DeleteHospital(Guid id);

    }
}
