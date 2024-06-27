using Leigth_Website.Models;
using Leigth_Website.Services.EmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
namespace Leigth_Website.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("/[Action]")]
        public async Task<IActionResult> SendEmail([FromBody] ContactFormModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Se necesitan todas las propiedades para el mensaje");
                }
                //Model Validation
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var EmailGotSent = await _emailService.SendEmail(model);
                if (!EmailGotSent)
                {
                    throw new SmtpException("Hubo un error al enviar el mensaje,porfa vuelve a intentarlo");
                }
                return Ok("¡El correo se envio con éxito!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Hubo un error al enviar el mensaje,porfa vuelve a intentarlo");
            }
        }

    }
}
