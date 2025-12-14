namespace PerFinanc.Web.Email
{
    public class SmtpOtions
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool UseSsl { get; set; } = true;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FromEmail { get; set; }
        public string FromName { get; set; }
    }
}
