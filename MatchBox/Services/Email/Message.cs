using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Services.Email
{
    public class Message
    {
        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<string>();

            To.AddRange(to);
            Subject = subject;
            Content = content;
        }
        
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    
    }
}
