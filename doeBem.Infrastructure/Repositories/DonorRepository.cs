using doeBem.Core.Entities;
using doeBem.Core.Interfaces;
using doeBem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace doeBem.Infrastructure.Repositories
{
    public class DonorRepository : IDonorRepository
    {
        private readonly MyDbContext _context;
        public DonorRepository(MyDbContext context) 
        {
            _context = context;
        }
       public async Task<Donor> GetByIdAsync(Guid id)
        {
            return await _context.Donors
                .Include(d => d.Donations)
                .ThenInclude(h => h.Hospital)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Donor>> GetAllAsync()
        {
            return await _context.Donors
                .Include(d => d.Donations)
                .ThenInclude(h => h.Hospital)
                .ToListAsync();
        }

        public async Task AddAsync(Donor donor)
        {
            await _context.Donors.AddAsync(donor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Donor donor)
        {
            _context.Donors.Update(donor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var Donor = await _context.Donors.FindAsync(id);
            if(Donor != null)
            {
                _context.Donors.Remove(Donor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Donor> GetByEmailAsync(string email)
        {
            return await _context.Donors.FirstOrDefaultAsync(e => e.Email == email); 
        }
    }
}
