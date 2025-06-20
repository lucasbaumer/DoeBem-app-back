using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Application.DTOS
{
    public class DonationForHospitalDTO
    { 
        public Guid Id { get; set; }
        public float Value { get; set; }
        public DateTime Date { get; set; }
        public Guid DonorId { get; set; }
        public string DonorName { get; set; }
    }
}
