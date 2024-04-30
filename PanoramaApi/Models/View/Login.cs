using System.ComponentModel.DataAnnotations;

namespace PanoramaApi.Models.View
{
    public class Login
    {
        [MinLength(3)]
        [Required]
        public string Username { get; set; }

        [MinLength(8)]
        [Required]
        public string Password { get; set; }
    }
}
