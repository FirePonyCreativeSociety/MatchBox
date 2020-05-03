using MatchBox.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Models
{
    public class AddClaimToUserModel : UsernameOrEmailModel
    {
        [Required]
        public Claim Claim { get; set; }
    }
}
