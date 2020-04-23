using System;
using System.Collections.Generic;

namespace MatchBox.Contracts
{
    public class User : EntityBase
    {        
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public bool IsDisabled { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
    }
}