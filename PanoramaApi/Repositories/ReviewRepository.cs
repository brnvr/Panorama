using PanoramaApi.Models.Entities;
using System.Xml.Linq;

namespace PanoramaApi.Repositories
{
    public class ReviewRepository : RepositoryBase<Review>
    {
        public ReviewRepository(AppDbContext dbContext) : base("review", dbContext, dbContext => dbContext.Reviews) { }

        public Review Find(int userId, int tmdbId)
        {
            var query = from m in Entities where m.UserId == userId && m.TmdbId == tmdbId select m;

            var review = query.FirstOrDefault();

            if (review is null)
            {
                throw new NotFoundException(EntityDescription, $"Review for movie {tmdbId} not found for user {userId}.");
            }

            return review;
        }

        public override Review Update(int id, Action<Review> updateFunction)
        {
            var review = base.Update(id, updateFunction);

            if (review.Rating < 1 || review.Rating > 10)
            {
                throw new ArgumentException($"Rating must be an integer between 1 and 10 (real: {review.Rating}).", nameof(review.Rating));
            }

            return review;
        }

        public bool Exists(int userId, int tmdbId)
        {
            var results = Entities.Where(x => x.UserId == userId && x.TmdbId == tmdbId).Select(x => new
            {
                x.Id
            });

            return results.Count() > 0;
        }
    }
}
