using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Application.DTOS
{
    public class DonationDTO
    {
        public float Value { get; set; }
        public string Date { get; set; }
        public Guid DonorId { get; set; }
        public Guid HospitalId { get; set; }
    }
}
