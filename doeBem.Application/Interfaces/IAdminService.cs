using doeBem.Application.DTOS;

namespace doeBem.Core.Interfaces
{
    public interface IAdminService
    {
        Task<Guid> RegisterAdmin(AdminCreateDTO adminCreateDto);
    }
}
