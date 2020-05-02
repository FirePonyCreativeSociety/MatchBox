using System.Threading.Tasks;

namespace MatchBox.Services.Email
{
    public interface IEmailSender
    {
        EmailConfiguration Configuration { get; }
        Task SendEmailAsync(Message message);
    }
}
