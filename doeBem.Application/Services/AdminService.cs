using doeBem.Application.DTOS;
using doeBem.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdminService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Guid> RegisterAdmin(AdminCreateDTO adminCreateDto)
        {
            var usuarioExistente = await _userManager.FindByEmailAsync(adminCreateDto.Email);

            if(usuarioExistente != null)
            {
                throw new Exception("Email já cadastrado!");
            }

            var admin = new IdentityUser
            {
                UserName = adminCreateDto.Email,
                Email = adminCreateDto.Email,
                PhoneNumber = adminCreateDto.Phone
            };

            var result = await _userManager.CreateAsync(admin, adminCreateDto.Password);

            if(!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Erro ao criar admin: {errors}");
            }

            await _userManager.AddToRoleAsync(admin, "Admin");
            return Guid.Parse(admin.Id);
        }
    }
}
