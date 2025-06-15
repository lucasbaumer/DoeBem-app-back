using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Application.DTOS
{
    public class HospitalDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public int CNES { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }
        public string? Description { get; set; }
    }
}
