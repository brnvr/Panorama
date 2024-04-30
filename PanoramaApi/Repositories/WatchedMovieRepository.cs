using PanoramaApi.Models.Entities;

namespace PanoramaApi.Repositories
{
    public class WatchedMovieRepository : RepositoryBase<WatchedMovie>
    {
        public WatchedMovieRepository(AppDbContext dbContext) : base("watched movie", dbContext, dbContext => dbContext.WatchedMovies) { }

        public WatchedMovie Find(int userId, int tmdbId)
        {
            var query = from m in Entities where m.UserId == userId && m.TmdbId == tmdbId select m;

            var movie = query.FirstOrDefault();

            if (movie is null)
            {
                throw new EntityNotFoundException(EntityDescription, $"Movie {tmdbId} is not in user {userId}'s \"watched\" list.");
            }

            return movie;
        }
    }
}