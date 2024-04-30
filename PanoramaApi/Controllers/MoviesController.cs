using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PanoramaApi.Extensions;
using PanoramaApi.Services;
using PanoramaApi.Tmdb;

namespace PanoramaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        WebServiceManager<AppDbContext> _webService;

        public MoviesController(AppDbContext dbContext)
        {
            _webService = new WebServiceManager<AppDbContext>(this, dbContext);
        }

        /// <summary>
        /// Get movie from TMDB
        /// </summary>
        /// <param name="id">TMDB movie id</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> Movie(int id)
        {
            return await _webService.Perform(async () => Ok(await new TmdbApi(new Uri(AppSettings.TmdbApiUrl), AppSettings.TmdbToken).GetMovie(id)));
        }

        /// <summary>
        /// Get user's review for movie
        /// </summary>
        /// <param name="id">TMDB movie id</param>
        [HttpGet("{id}/Review")]
        public async Task<IActionResult> GetReview(int id)
        {
            return Ok();
        }

        /// <summary>
        /// Add or edit review content
        /// </summary>
        /// <param name="id">TMDB movie id</param>
        /// <param name="content">Content</param>
        [HttpPatch("{id}/Review/Content")]
        public async Task<IActionResult> EditContent(int id, string content)
        {
            return await _webService.Perform(async dbContext =>
            {
                await new MovieService(dbContext).EditReview(User.GetId(), id, review => review.Content = content);

                return NoContent();
            });
        }

        /// <summary>
        /// Rate a movie
        /// </summary>
        /// <param name="id">Movie id</param>
        /// <param name="rating">Rating from 1 to 10 (if null, removes rating)</param>
        [HttpPatch("{id}/Review/Rating")]
        public async Task<IActionResult> EditRating(int id, int rating)
        {
            return await _webService.Perform(async dbContext =>
            {
                await new MovieService(dbContext).EditReview(User.GetId(), id, review => review.Rating = rating);

                return NoContent();
            });
        }
    }
}