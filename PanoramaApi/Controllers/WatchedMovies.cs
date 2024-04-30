using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PanoramaApi.Services;
using PanoramaApi.Extensions;

namespace PanoramaApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WatchedMoviesController : ControllerBase
    {
        WebServiceManager<AppDbContext> _webService;

        public WatchedMoviesController(AppDbContext dbContext)
        {
            _webService = new WebServiceManager<AppDbContext>(this, dbContext);
        }

        /// <summary>
        /// Get current user's watched movies
        /// </summary>
        [HttpGet]
        public IActionResult GetMovies()
        {
            return _webService.Perform(dbContext => Ok(new UserService(dbContext).GetMovies(User.GetId())));
        }

        /// <summary>
        /// Add a movie to user's "watched" list
        /// </summary>
        /// <param name="id">TMDB movie id</param>
        [HttpPost("{id}")]
        public async Task<IActionResult> AddMovie(int id)
        {
            return await _webService.Perform(async dbContext => await new UserService(dbContext).AddMovie(User.GetId(), id));
        }

        /// <summary>
        /// Remove a movie from user's "watched" list
        /// </summary>
        /// <param name="id">TMDB movie id</param>
        [HttpDelete("{id}")]
        public IActionResult RemoveMovie(int id)
        {
            return _webService.Perform(dbContext => new UserService(dbContext).RemoveMovie(User.GetId(), id));
        }
    }
}