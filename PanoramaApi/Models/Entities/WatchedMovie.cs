namespace PanoramaApi.Models.Entities
{
    public class WatchedMovie : IEntity
    {
        public int Id { get; set; }
        public int TmdbId { get; set; }
        public int UserId { get; set; }
    }
}
