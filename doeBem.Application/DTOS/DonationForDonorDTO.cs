using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Application.DTOS
{
    public class DonationForDonorDTO
    {
        public Guid Id { get; set; }
        public float Value { get; set; }
        public DateTime Date { get; set; }
        public Guid HospitalId { get; set; }
        public string HospitalName { get; set; }
    }
}
