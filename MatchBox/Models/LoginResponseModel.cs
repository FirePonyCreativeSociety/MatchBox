using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Models
{
    public class LoginResponseModel
    {
        public string Jwt { set; get; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}