using System.Threading.Tasks;

namespace Users.Api.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string body)
        {
            return Task.CompletedTask;
        }
    }
}