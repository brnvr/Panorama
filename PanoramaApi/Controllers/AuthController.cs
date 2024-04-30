using Microsoft.AspNetCore.Mvc;
using PanoramaApi.Services;
using PanoramaApi.Models.View;
using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace PanoramaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        WebServiceManager<AppDbContext> _webService;

        public AuthController(AppDbContext dbContext)
        {
            _webService = new WebServiceManager<AppDbContext>(this, dbContext);
        }

        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="user">User credentials</param>
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [HttpPost("[Action]")]
        public IActionResult Login(Login user)
        {
            return _webService.Perform(dbContext =>
            {
                var token = new AuthenticationService(dbContext).Login(user.Username, user.Password);

                Response.Cookies.Append("AccessToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, 
                    SameSite = SameSiteMode.Strict 
                });
            });
        }

        /// <summary>
        /// Authenticate user and returns JWT (system admin)
        /// </summary>
        /// <param name="user">User credentials</param>
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [HttpPost("Login/Token")]
        public IActionResult LoginGetToken(Login user)
        {
            return _webService.Perform(dbContext =>
            {
                var token = new AuthenticationService(dbContext).Login(user.Username, user.Password, "system_admin");

                Response.Cookies.Append("AccessToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

                return Ok(token);
            });
        }
    }
}