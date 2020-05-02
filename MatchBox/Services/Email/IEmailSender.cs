using System.Threading.Tasks;

namespace MatchBox.Services.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
    }
}
