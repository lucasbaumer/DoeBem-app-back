using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doeBem.Application.DTOS
{
    public class AdminCreateDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }

        [Required]
        [MinLength(6, ErrorMessage ="A senha deve ter no minimo 6 caracteres, uma letra maiuscula, um numero e um caracter especial")]
        public string Password { get; set; }
    }
}
