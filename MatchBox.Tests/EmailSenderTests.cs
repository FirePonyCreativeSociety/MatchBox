using MatchBox.Services.Email;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MatchBox.Tests
{
    public class EmailSenderTests
    {
        [Fact]
        public async Task SendEmailWorks()
        {
            var cfg = new EmailConfiguration
            {                 
                UserName = "adminuser@devtests.local",
                Password = "password",
                From = "donotreply@devtests.local",
                SmtpServer = "devtests.local",
                Port = 25,
                UseSSL = false, 
            };

            var msg = new Message(new[] { "user1@devtests.local", "user2@devtests.local", "adminuser@devtests.local" }, "Test Email!", "It's " + DateTime.Now.ToString());

            var sender = new EmailSender(cfg);
            await sender.SendEmailAsync(msg);
        }
    }
}
