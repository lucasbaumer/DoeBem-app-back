using doeBem.Core.Entities;
using doeBem.Core.Interfaces;
using doeBem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Infrastructure.Repositories
{
    public class HospitalRepository : IHospitalRepository
    {
        private readonly MyDbContext _context;

        public HospitalRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Hospital hospital)
        {
            await _context.Hospitals.AddAsync(hospital);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var Hospital = await _context.Hospitals.FindAsync(id);
            if(Hospital != null)
            {
                _context.Hospitals.Remove(Hospital);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Hospital>> GetAllAsync()
        {
            return await _context.Hospitals
                .Include(h => h.ReceivedDonations)
                .ThenInclude(d => d.Donor)
                .ToListAsync();
        }

        public async Task<Hospital> GetByCnesAsync(int cnes)
        {
            return await _context.Hospitals
                .Include(h => h.ReceivedDonations)
                .ThenInclude(d => d.Donor)
                .FirstOrDefaultAsync(c => c.CNES == cnes);
        }

        public async Task<Hospital> GetByIdAsync(Guid id)
        {
            return await _context.Hospitals
                .Include(h => h.ReceivedDonations)
                .ThenInclude(d => d.Donor)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task UpdateAsync(Hospital hospital)
        {
            _context.Hospitals.Update(hospital);
             await _context.SaveChangesAsync();
        }
    }
}
