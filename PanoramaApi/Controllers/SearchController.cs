using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PanoramaApi.Tmdb;
using System.ComponentModel.DataAnnotations;

namespace PanoramaApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        WebServiceManager _webService;

        public SearchController()
        {
            _webService = new WebServiceManager(this);
        }

        /// <summary>
        /// Search for movies that match query
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="page">Results' page</param>
        [HttpGet("Movies")]
        public async Task<IActionResult> SearchMovie([Required]string query, int page=1)
        {
            return await _webService.Perform(async () => Ok(await new TmdbApi(new Uri(AppSettings.TmdbApiUrl), AppSettings.TmdbToken).SearchMovies(query, page)));
        } 
    }
}