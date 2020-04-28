using System;
using System.Collections.Generic;
using System.Text;

namespace MatchBox.Data.Models
{
    public class DbEvent
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Title { get; set; }        
    }
}
