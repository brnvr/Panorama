using PanoramaApi.Gemini;
using PanoramaApi.Models.Entities;
using PanoramaApi.Repositories;
using PanoramaApi.Tmdb;
using PanoramaApi.Tmdb.Models;

namespace PanoramaApi.Services
{
    public class MovieService
    {
        AppDbContext _dbContext;

        public MovieService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task EditReview(int userId, int tmdbId, Action<Review> updateFunction)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var repo = new ReviewRepository(_dbContext);
                var review = await GetOrCreateReview(repo, userId, tmdbId);

                repo.Update(review.Id, updateFunction);
                RemoveReviewIfEmpty(repo, review);

                _dbContext.SaveChanges();
                transaction.Commit();
            }
        }

        protected async Task<Review> GetOrCreateReview(ReviewRepository repo, int userId, int tmdbId)
        {
            if (repo.Exists(userId, tmdbId))
            {
               return repo.Find(userId, tmdbId);
            }
               
            return await CreateReview(userId, tmdbId);
        } 

        protected void RemoveReviewIfEmpty(ReviewRepository repo, Review review)
        {
            if ((review.Content == null || review.Content.Replace(" ", "") == string.Empty) && review.Rating == null)
            {
                repo.Remove(review.Id);
            }
        }

        protected async Task<Review> CreateReview(int userId, int tmdbId)
        {
            var tmdb = new TmdbApi(new Uri(AppSettings.TmdbApiUrl), AppSettings.TmdbToken);

            await tmdb.GetMovie(tmdbId);

            var review = new Review
            {
                UserId = userId,
                TmdbId = tmdbId
            };

            new ReviewRepository(_dbContext).Add(review);

            _dbContext.SaveChanges();

            return review;
        }
    }
}
