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
        static EmailConfiguration GetLocalServerConfiguration()
        { 
            return new EmailConfiguration
            {
                UserName = "adminuser@devtests.local",
                Password = "password",
                From = "donotreply@devtests.local",
                SmtpServer = "devtests.local",
                Port = 25,
                UseSSL = false,
            };
        }

        static EmailConfiguration GetVDFServerConfiguration()
        {
            return new EmailConfiguration
            {
                UserName = "vdfbot@playadelfuego.org",
                Password = "0xoq0ww;tyk&!u@YHJV0sCdOS&30kP",
                From = "dontrespond@playdelfuego.org",
                SmtpServer = "smtp.dreamhost.com",
                Port = 465,
                UseSSL = true
            };
        }



        Message GetMessage()
        {
            return new Message(new[]
                {
                "user1@devtests.local",
                "user2@devtests.local",
                "adminuser@devtests.local",
                //"afederici75@gmail.com",
                //"afederici@codysystems.com"
                },
                "Test Email from Visual Studio tests...",
                "From Visual Studio Tests @" + DateTime.Now.ToString()
                );
        }

        EmailConfiguration CurrentConfiguration = GetVDFServerConfiguration();

        [Fact]
        public async Task MimeKitSendEmailWorks()
        {            
            var sender = new MimeKitEmailSender(CurrentConfiguration);
            await sender.SendEmailAsync(GetMessage());
        }

        [Fact]
        public async Task SystemSendEmailWorks()
        {
            var sender = new SystemKitEmailSender(CurrentConfiguration);
            await sender.SendEmailAsync(GetMessage());
        }
    }
}
