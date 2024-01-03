namespace Infra.ExternalServices.MailSender.Configurations
{

    public class ILinxIOSmtpConfig
    {

        public string Username { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public bool Ssl { get; set; }

    }

}