using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PanoramaApi.Extensions;
using PanoramaApi.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace PanoramaApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        WebServiceManager<AppDbContext> _webService;
        public UserController(AppDbContext dbContext)
        {
            _webService = new WebServiceManager<AppDbContext>(this, dbContext);
        }

        /// <summary>
        /// Get logged user's info
        /// </summary>
        
        [HttpGet]
        public IActionResult Get()
        {
            var user = new
            {
                Email = User.GetClaim(ClaimTypes.Email).Value,
                Username = User.GetClaim(ClaimTypes.Name).Value,
                Role = User.GetClaim(ClaimTypes.Role).Value,
                PicturePath = User.GetClaim("picturePath").Value
            };

            return new JsonResult(user);
        }

        /// <summary>
        /// Set user password
        /// </summary>
        /// <param name="password">New password</param>
        [HttpPatch("Password")]
        public IActionResult EditPassword([Required]string password)
        {
            return _webService.Perform(dbContext =>
            {
                new UserService(dbContext).EditPassword(User.GetId(), password);

                return NoContent();
            });
        }

        /// <summary>
        /// Set user profile picture
        /// </summary>
        /// <param name="file">Picture file (jpg/jpeg/png)</param>
        /// <returns></returns>
        [HttpPatch("Picture")]
        public async Task<IActionResult> UploadImage([Required]IFormFile file)
        {
            return await _webService.Perform(async dbContext =>
            {
                var picturePath = await new UserService(dbContext).EditPicture(User.GetId(), file);

                return Ok(new { picturePath });
            });
        }
    }
}