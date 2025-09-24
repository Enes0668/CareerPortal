using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");
        var smtp = new SmtpClient(emailSettings["SmtpServer"], int.Parse(emailSettings["Port"]))
        {
            Credentials = new NetworkCredential(emailSettings["SenderEmail"], emailSettings["Password"]),
            EnableSsl = true
        };

        var mail = new MailMessage
        {
            From = new MailAddress(emailSettings["SenderEmail"], emailSettings["SenderName"]),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };

        mail.To.Add(email);
        return smtp.SendMailAsync(mail);
    }
}
