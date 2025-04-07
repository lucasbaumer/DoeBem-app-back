using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Application.DTOS
{
    public class HospitalUpdateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int CNES { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Phone { get; set; }

        public string? Description { get; set; }
    }
}
