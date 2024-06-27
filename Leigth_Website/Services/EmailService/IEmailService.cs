using Leigth_Website.Models;

namespace Leigth_Website.Services.EmailService
{
    public interface IEmailService
    {
        Task<bool> SendEmail(ContactFormModel model);
    }
}
