using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MatchBox.Data.Models
{
    public class DbEvent
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [Required]
        public string Title { get; set; }        
        public byte[] Data { get; set; }
    }
}