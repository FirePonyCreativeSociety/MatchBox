using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Models
{
    public class RegisterNewUserModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }

        public ICollection<Claim> Claims { get; set; } = new List<Claim>();
    }
}
