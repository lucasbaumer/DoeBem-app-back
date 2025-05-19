using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Application.DTOS
{
    public class HospitalWithDonationsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int CNES { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string? Description { get; set; }

        public List<DonationForHospitalDTO> ReceivedDonations { get; set; } = new();
    }
}

