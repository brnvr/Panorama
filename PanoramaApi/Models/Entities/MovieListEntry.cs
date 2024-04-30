using PanoramaApi.Tmdb.Models;

namespace PanoramaApi.Models.Entities
{
    public class MovieListEntry : IEntity
    {
        public int Id { get; private set; }
        public int TmdbId { get; set; }
        public int ListId { get; set; }
    }
}
