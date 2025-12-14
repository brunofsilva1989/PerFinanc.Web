
using MailKit.Security;
using Microsoft.Extensions.Options;

namespace PerFinanc.Web.Email
{
    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpOtions _options;

        public SmtpEmailService(IOptions<SmtpOtions> opt)
        {
            _options = opt.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody, CancellationToken ct = default)
        {
            var message = new MimeKit.MimeMessage();
            message.From.Add(new MimeKit.MailboxAddress(_options.FromName, _options.FromEmail));
            message.To.Add(MimeKit.MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            message.Body = new MimeKit.BodyBuilder
            {
                HtmlBody = htmlBody
            }.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();

            var scure = _options.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTlsWhenAvailable;

            await client.ConnectAsync(_options.Host, _options.Port, scure, ct);
            await client.AuthenticateAsync(_options.User, _options.Password, ct);
            await client.SendAsync(message, ct);
            await client.DisconnectAsync(true, ct);
        }        
    }
}
