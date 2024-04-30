using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PanoramaApi.Extensions;
using PanoramaApi.Models.View;
using PanoramaApi.Repositories;
using PanoramaApi.Services;
using System.Security.Claims;

namespace PanoramaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        WebServiceManager<AppDbContext> _webService;
        public UsersController(AppDbContext dbContext)
        {
            _webService = new WebServiceManager<AppDbContext>(this, dbContext);
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="registration">User information</param>
        [HttpPost]
        public IActionResult Register(Registration registration)
        {
            return _webService.Perform(dbContext =>
            {
                var id = new UserService(dbContext).Register(registration);

                return CreatedAtAction(nameof(GetById), new { id }, new { id });
            });
        }

        /// <summary>
        /// Get user info by id (system admin)
        /// </summary>
        /// <param name="id">User id</param>
        [Authorize(Roles = "system_admin")]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return _webService.Perform(dbContext => Ok(new UserRepository(dbContext).FindById(id)));
        } 
    }
}