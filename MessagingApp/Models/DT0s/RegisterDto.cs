using System.ComponentModel.DataAnnotations;

namespace MessagingApp.Models.DT0s
{
    public class RegisterDto
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}
