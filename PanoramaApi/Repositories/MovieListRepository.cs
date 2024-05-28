using PanoramaApi.Models.Entities;

namespace PanoramaApi.Repositories
{
    public class MovieListRepository : RepositoryBase<MovieList>
    {
        public MovieListRepository(AppDbContext dbContext) : base("movie list", dbContext, dbContext => dbContext.MovieLists) { }

        public MovieList Find(int userId, string name)
        {
            var query = from l in Entities
                        where l.UserId == userId && l.Name == name
                        select l;

            var list = query.FirstOrDefault();

            if (list == null)
            {
                throw new NotFoundException(EntityDescription, $"Movie list {name} not found for user {userId}.");
            }

            return list;
        }
    }
}