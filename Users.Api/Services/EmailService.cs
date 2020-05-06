using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Users.Api.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string body)
        {
            SmtpClient sc = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Credentials = new NetworkCredential("fatih.erol108@gmail.com", "Application108")
            };

            MailMessage mail = new MailMessage
            {
                From = new MailAddress("fatih.erol108@gmail.com", "Ekranda Görünecek İsim")
            };

            mail.To.Add(email);

            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = body;

            sc.Send(mail);

            return Task.CompletedTask;
        }
    }
}