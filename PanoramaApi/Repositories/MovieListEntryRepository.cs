using PanoramaApi.Models.Entities;

namespace PanoramaApi.Repositories
{
    public class MovieListEntryRepository : RepositoryBase<MovieListEntry>
    {
        public MovieListEntryRepository(AppDbContext dbContext) : base("movie list entry", dbContext, dbContext => dbContext.MovieListEntries) { }

        public MovieListEntry Find(int listId, int tmdbId)
        {
            var query = from l in Entities
                        where l.ListId == listId && l.TmdbId == tmdbId
                        select l;

            var entry = query.FirstOrDefault();

            if (entry == null)
            {
                throw new NotFoundException(EntityDescription, $"movie {tmdbId} not found in list {listId}.");
            }

            return entry;
        }
    }
}