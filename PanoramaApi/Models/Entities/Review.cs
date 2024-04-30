using System.ComponentModel.DataAnnotations;

namespace PanoramaApi.Models.Entities
{
    public class Review : IEntity
    {
        public int Id { get; private set; }
        public int UserId { get; set; }
        public int TmdbId { get; set; }
        public int? Rating { get; set; }
        public string? Content { get; set; }
    }
}
