namespace Leigth_Website.Models
{
    public class SMTPSettingsModel
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
