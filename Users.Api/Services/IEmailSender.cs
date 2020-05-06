using System.Threading.Tasks;

namespace Users.Api.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string body);
    }
}