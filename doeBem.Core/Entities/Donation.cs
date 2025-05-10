using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Core.Entities
{
    public class Donation
    {
        public Guid Id { get; set; }
        public float Value { get; set; }
        public DateTime Date { get; set; }

        public Guid DonorId { get; set; }
        public Donor Donor { get; set; }

        public Guid HospitalId { get; set; }
        public Hospital Hospital { get; set; }

    }
}
