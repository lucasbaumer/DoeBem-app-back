using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Application.DTOS
{
    public class DonationDTO
    {
        public Guid Id { get; set; }
        [Required]
        public float Value { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public Guid DonorId { get; set; }
        public string? DonorName { get; set; }
        [Required]
        public Guid HospitalId { get; set; }
        public string? HospitalName { get; set;}
        
    }
}
