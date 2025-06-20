using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Application.DTOS
{
    public class DonorDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
    }
}
