using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Models
{
    public class RemoveClaimFromUserModel : UsernameOrEmailModel
    {
        [Required]
        public string ClaimType { get; set; }
    }
}
