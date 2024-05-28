using Microsoft.AspNetCore.Mvc;
using PanoramaApi.Services;
using PanoramaApi.Models.View;
using System.Net;
using Swashbuckle.AspNetCore.Annotations;
using PanoramaApi.Gemini.Models;
using PanoramaApi.Gemini;
using PanoramaApi.Extensions;

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
        /// <response code="204">Successful login</response>
        /// <response code="401">Incorrect username or password</response>
        [HttpPost("[Action]")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
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
        /// Authenticate user and return JWT (system admin)
        /// </summary>
        /// <param name="user">User credentials</param>
        /// <response code="200">Successful login</response>
        /// <response code="401">Incorrect username or password</response>
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