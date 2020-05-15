using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.API.Models
{
    public class SendEmailModel
    {
        [Required]
        public IList<string> To { get; set; }

        [Required]
        public string Subject { get; set; }
        
        public string Content { get; set; }        
    }
}
