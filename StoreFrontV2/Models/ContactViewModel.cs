using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StoreFrontV2.Models
{
    [Keyless]
    public class ContactViewModel
    {
        [Required(ErrorMessage = "*Name is required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "*Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "*Subject is required")]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = "*Message is required")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; } = null!;
    }
}
