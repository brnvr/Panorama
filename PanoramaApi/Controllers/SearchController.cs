using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PanoramaApi.Services;
using PanoramaApi.Tmdb;
using System.ComponentModel.DataAnnotations;

namespace PanoramaApi.Controllers
{
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
        /// <param name="year">Release year</param>
        /// <param name="page">Results' page</param>
        [HttpGet("Movies")]
        public async Task<IActionResult> SearchMovie([Required]string query, int? year, int page=1)
        {
            var tmdb = new TmdbApi(new Uri(AppSettings.TmdbApiUrl), AppSettings.TmdbToken);

            return await _webService.Perform(async () => Ok(await tmdb.SearchMovies(query, page, year)));
        }

        /// <summary>
        /// Search for movies similar to selected movie
        /// </summary>
        /// <param name="id">TMDB movie id</param>
        /// <param name="number">Number of results</param>
        /// <returns></returns>
        [HttpGet("Movies/Similar/{id}")]
        public async Task<IActionResult> SearchSimilar(int id, [Required] int number=10)
        {
            return await _webService.Perform(async () =>
            {
                var result = await new MovieRecommendationsService().FindSimilar(id, number);

                return Ok(result);
            });
        }
    }
}