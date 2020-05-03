using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Services.Email
{
    public class EmailConfiguration
    {
        public string From { get; set; } = "donotreply@devtests.local";
        public string SmtpServer { get; set; } = "localhost";
        public int Port { get; set; } = 25;
        public string UserName { get; set; } = "adminuser@devtests.local";
        public string Password { get; set; } = "password";
        public bool UseSSL { get; set; } = true;
    }
}