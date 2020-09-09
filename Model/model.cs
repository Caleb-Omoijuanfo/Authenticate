using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticate.Model
{
    public class UserModel
    {
        [Required]
        public string Username { get; set;  }

        [Required]
        public string Email { get; set; }
    }
}
