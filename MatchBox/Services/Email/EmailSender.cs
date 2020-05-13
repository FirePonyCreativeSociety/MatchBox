using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace MatchBox.Services.Email
{
    public class EmailSender : IEmailSender
    {        
        public EmailSender(EmailConfiguration configuration)
        {
            Configuration = configuration;
        }

        public EmailConfiguration Configuration { get; }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(Configuration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }
        
        public async Task SendEmailAsync(Message message)
        {            
            using (var client = new SmtpClient(new MailKit.ProtocolLogger("smtp.log")))
            {
                try
                {
                    await client.ConnectAsync(Configuration.SmtpServer, Configuration.Port, Configuration.UseSSL);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(Configuration.UserName, Configuration.Password);

                    var msg = CreateEmailMessage(message);
                    await client.SendAsync(msg);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

    }
}
