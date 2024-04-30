using System.ComponentModel.DataAnnotations;

namespace PanoramaApi.Models.View
{
    public class Registration
    {
        [Required]
        [EmailAddress]
        [MaxLength(64)]
        public string Email { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(32)]
        public string Username { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(72)]
        public string Password { get; set; }    
    }
}
