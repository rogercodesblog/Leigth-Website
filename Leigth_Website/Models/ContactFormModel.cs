using System.ComponentModel.DataAnnotations;

namespace Leigth_Website.Models
{
    public class ContactFormModel
    {
        [Required(ErrorMessage = "Name Required")]
        [MaxLength(50, ErrorMessage = "Name cannot be longer than 50 Characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email Required")]
        [MaxLength(300, ErrorMessage = "Email cannot be longer that 300 characters")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email is not valid")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Message Required")]
        [MaxLength(1500, ErrorMessage = "Message cannot be longer than 1500 Characters")]
        public string Message { get; set; }
    }
}
