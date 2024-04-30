using System.ComponentModel.DataAnnotations;

namespace PanoramaApi.Models.Entities
{
    public class User : IEntity
    {
        public int Id { get; private set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }     
        public string? PicturePath { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
