using System.ComponentModel.DataAnnotations;

namespace PanoramaApi.Models.View
{
    public class ListCreation
    {
        [Required]
        [MinLength(3)]
        [MaxLength(32)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}
