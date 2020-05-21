using MailKit.Net.Smtp;
using MimeKit;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Services.Email
{
    public class MimeKitEmailSender : IEmailSender
    {
        public MimeKitEmailSender(EmailConfiguration configuration)
        {
            Configuration = configuration;
        }

        public EmailConfiguration Configuration { get; }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(Configuration.From));
            var tmp = message.To.Select(to => new MailboxAddress(to));
            emailMessage.To.AddRange(tmp);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        public async Task SendEmailAsync(Message message)
        {
            // See new 'using' declaration. Cool stuff!
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-8.0/using            
            using var client = new SmtpClient(new MailKit.ProtocolLogger("smtp.log")); 

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
