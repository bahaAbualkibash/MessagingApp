using System.ComponentModel.DataAnnotations;

namespace MessagingApp.Models.DT0s
{
    public class RegisterDto
    {
        [Required]
        public string username { get; set; }
        
        [Required]
        [MinLength(8)]
        public string password { get; set; }

        [Required]
        public string KnownAs { get; set; }
        
        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
