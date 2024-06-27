using Leigth_Website.Models;
using System.Net;
using System.Net.Mail;

namespace Leigth_Website.Services.EmailService
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmail(ContactFormModel model)
        {
            try
            {
                SMTPSettingsModel smtpsettings = GetSMTPSettings();

                using (MailMessage email = new MailMessage(smtpsettings.FromAddress, smtpsettings.ToAddress))
                {
                    email.Subject = "Correo para Leigth!";
                    email.Body = $"Por parte de: {model.Name}<br/>Email: {model.Email}<br/>Mensaje: {model.Message}";
                    email.IsBodyHtml = true;
                    email.BodyEncoding = System.Text.Encoding.UTF8;

                    using (SmtpClient smtp = new SmtpClient())
                    {
                        NetworkCredential NetworkCred = new NetworkCredential(smtpsettings.UserName, smtpsettings.Password);
                        smtp.Credentials = NetworkCred;
                        smtp.Host = smtpsettings.Host;
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Port = smtpsettings.Port;
                        await smtp.SendMailAsync(email);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private SMTPSettingsModel GetSMTPSettings()
        {
            //Read SMTP settings from AppSettings.json.
            return new SMTPSettingsModel()
            {
                Host = _configuration.GetValue<string>("Smtp:Server"),
                Port = _configuration.GetValue<int>("Smtp:Port"),
                FromAddress = _configuration.GetValue<string>("Smtp:FromAddress"),
                ToAddress = _configuration.GetValue<string>("Smtp:ToAddress"),
                UserName = _configuration.GetValue<string>("Smtp:UserName"),
                Password = _configuration.GetValue<string>("Smtp:Password")
            };
        }
    }
}
