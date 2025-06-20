using doeBem.Application.DTOS;
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
    public class DonationRepository : IDonationRepository
    {

        private readonly MyDbContext _context;
        public DonationRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Donation donation)
        {
            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if(donation != null)
            {
                _context.Donations.Remove(donation);
                await _context.SaveChangesAsync();

            }
        }

        public async Task<IEnumerable<Donation>> GetAllAsync()
        {
            return await _context.Donations
                .Include(d => d.Donor)
                .Include(d => d.Hospital)
                .ToListAsync();
        }

        public async Task<IEnumerable<Donation>> GetByHospitalIdAsync(Guid hospitalId)
        {
            return await _context.Donations
                .Include(d => d.Donor)
                .Include(d => d.Hospital)
                .Where(d => d.HospitalId == hospitalId)
                .ToListAsync();
        }

        public async Task<Donation> GetByIdAsync(Guid id)
        {
            return await _context.Donations
                .Include(d => d.Donor)
                .Include(d => d.Hospital)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task UpdateAsync(Donation donation)
        {
            _context.Donations.Update(donation);
            await _context.SaveChangesAsync();
        }
    }
}
