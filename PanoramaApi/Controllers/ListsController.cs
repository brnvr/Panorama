using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PanoramaApi.Extensions;
using PanoramaApi.Models.View;
using PanoramaApi.Services;
using System.ComponentModel.DataAnnotations;

namespace PanoramaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ListsController : ControllerBase
    {
        WebServiceManager<AppDbContext> _webService;

        public ListsController(AppDbContext dbContext)
        {
            _webService = new WebServiceManager<AppDbContext>(this, dbContext);
        }

        /// <summary>
        /// Get all current user's lists
        /// </summary>
        /// <param name="nameFilter">Search query (optional)</param>
        [HttpGet]
        public IActionResult Find(string? query)
        {
            return _webService.Perform(dbContext =>
            {
                return Ok(new MovieListService(dbContext).FindList(User.GetId(), query ?? ""));
            });
        }

        /// <summary>
        /// Get user list by name
        /// </summary>
        /// <param name="name">List name</param>
        [HttpGet("{name}")]
        public async Task<IActionResult> GetList(string name)
        {
            return await _webService.Perform(async dbContext =>
            {
                return Ok(await new MovieListService(dbContext).GetMovies(User.GetId(), name));
            });
        }

        /// <summary>
        /// Create new list
        /// </summary>
        /// <param name="list">List info</param>
        [HttpPost]
        public IActionResult CreateList([Required] ListCreation list)
        {
            return _webService.Perform(dbContext =>
            {
                var name = new MovieListService(dbContext).CreateList(User.GetId(), list.Title, list.Description);

                return CreatedAtAction(nameof(GetList), new { name }, new { name });
            });
        }

        /// <summary>
        /// Remove list
        /// </summary>
        /// <param name="name">List name</param>
        /// <returns></returns>
        [HttpDelete("{name}")]
        public IActionResult RemoveList([Required] string name)
        {
            return _webService.Perform(dbContext =>
            {
                new MovieListService(dbContext).RemoveList(User.GetId(), name);
            });
        }

        /// <summary>
        /// Edit list title
        /// </summary>
        /// <param name="name">List name</param>
        /// <param name="title">List title</param>
        /// <returns></returns>
        [HttpPatch("{name}/Title")]
        public IActionResult EditTitle(string name, string title)
        {
            return _webService.Perform(dbContext =>
            {
                var list = new MovieListService(dbContext).EditList(User.GetId(), name, l => l.Title = title);

                return Ok(new { list.Name });
            });
        }

        /// <summary>
        /// Edit list description
        /// </summary>
        /// <param name="name">List name</param>
        /// <param name="description">List description</param>
        /// <returns></returns>
        [HttpPatch("{name}/Description")]
        public IActionResult EditDescription(string name, string description)
        {
            return _webService.Perform(dbContext =>
            {
                new MovieListService(dbContext).EditList(User.GetId(), name, l => l.Description = description);
            });
        }

        /// <summary>
        /// Add movie to list
        /// </summary>
        /// <param name="name">List name</param>
        /// <param name="movieId">TMDB movie id</param>
        /// <returns></returns>
        [HttpPost("Movie/{name}/{movieId}")]
        public async Task<IActionResult> AddMovie(string name, int movieId)
        {
            return await _webService.Perform(async dbContext =>
            {
                await new MovieListService(dbContext).AddMovie(User.GetId(), name, movieId);
            });
        }

        /// <summary>
        /// Remove movie from list
        /// </summary>
        /// <param name="name">List name</param>
        /// <param name="movieId">TMDB movie id</param>
        /// <returns></returns>
        [HttpDelete("Movie/{name}/{movieId}")]
        public IActionResult RemoveMovie(string name, int movieId)
        {
            return _webService.Perform(dbContext =>
            {
                new MovieListService(dbContext).RemoveMovie(User.GetId(), name, movieId);
            });
        }
    }
}