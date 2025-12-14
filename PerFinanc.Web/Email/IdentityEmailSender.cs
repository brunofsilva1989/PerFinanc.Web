using Microsoft.AspNetCore.Identity.UI.Services;

namespace PerFinanc.Web.Email
{
    public class IdentityEmailSender : IEmailSender
    {
        private readonly IEmailService _emailService;
        public IdentityEmailSender(IEmailService email)
        {
            _emailService = email;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage) => _emailService.SendEmailAsync(email, subject, htmlMessage);
        
    }
}
