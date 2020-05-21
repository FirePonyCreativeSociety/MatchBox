using System.Net.Mail;
using System.Threading.Tasks;

namespace MatchBox.Services.Email
{
    public class SystemKitEmailSender : IEmailSender
    {
        public SystemKitEmailSender(EmailConfiguration configuration)
        {
            Configuration = configuration;
        }

        public EmailConfiguration Configuration { get; }

        private MailMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MailMessage
            {
                From = new MailAddress(Configuration.From)
            };

            foreach (var to in message.To)
                emailMessage.To.Add(new MailAddress(to));

            emailMessage.Subject = message.Subject;
            emailMessage.Body = message.Content;// new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            emailMessage.IsBodyHtml = false;

            return emailMessage;
        }

        public Task SendEmailAsync(Message message)
        {
            SmtpClient SmtpServer = new SmtpClient(Configuration.SmtpServer)
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential(Configuration.UserName, Configuration.Password),
                EnableSsl = Configuration.UseSSL
            };

            var msg = CreateEmailMessage(message);
            SmtpServer.Send(msg);

            return Task.CompletedTask;            
        }        
    }
}
