using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Models
{
    public class QueryModel
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string[] Include { get; set; }
        public string[] SortBy { get; set; }        
    }
}
