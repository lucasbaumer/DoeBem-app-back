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
        Task<IEnumerable<HospitalWithDonationsDto>> GetAllWithDonationsAsync();
        Task<IEnumerable<HospitalDTO>> GetAllHospitalsAsync();
        Task<HospitalWithDonationsDto> GetByIdWithDonationsAsync(Guid id);
        Task<HospitalDTO> GetHospitalByIdAsync(Guid id);
        Task<Guid> RegisterHospital(HospitalCreateDto registerHospitalDto);
        Task<bool> UpdateHospital(Guid id, HospitalUpdateDto updateHospitalDto);
        Task<bool> DeleteHospital(Guid id);
    }
}
