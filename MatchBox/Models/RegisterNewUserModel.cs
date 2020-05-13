using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MatchBox.Models
{
    public class RegisterNewUserModel : UpdateUserModel
    {
        [Required]
        public string Password { get; set; }
        
        public string AccessLevels { get; set; }
    }
}
