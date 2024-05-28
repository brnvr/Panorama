using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PanoramaApi
{
    public class WebServiceManager
    {
        protected ControllerBase Controller { get; set; }

        public WebServiceManager(ControllerBase controller)
        {
            Controller = controller;
        }

        public IActionResult Perform(Func<IActionResult> callback)
        {
            if (!Controller.ModelState.IsValid)
            {
                return Controller.BadRequest(Controller.ModelState);
            }

            try
            {
                return callback();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        public IActionResult Perform(Action callback)
        {
            if (!Controller.ModelState.IsValid)
            {
                return Controller.BadRequest(Controller.ModelState);
            }

            try
            {
                callback();

                return Controller.NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        public async Task<IActionResult> Perform(Func<Task<IActionResult>> callback)
        {
            if (!Controller.ModelState.IsValid)
            {
                return Controller.BadRequest(Controller.ModelState);
            }

            try
            {
                return await callback();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        public async Task<IActionResult> Perform(Func<Task> callback)
        {
            if (!Controller.ModelState.IsValid)
            {
                return Controller.BadRequest(Controller.ModelState);
            }

            try
            {
                await callback();

                return Controller.NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        protected IActionResult HandleException(Exception ex)
        {
            if (ex is DbUpdateException dbEx)
            {
                return Controller.UnprocessableEntity((dbEx.InnerException ?? dbEx).Message);
            }
            else if (ex is HttpRequestException httpEx)
            {
                return Controller.StatusCode((int)(httpEx.StatusCode ?? System.Net.HttpStatusCode.BadRequest), httpEx.Message);
            }
            else if (ex is AuthenticationException authEx)
            {
                return Controller.Unauthorized(authEx.Message);
            }
            else if (ex is NotFoundException entryEx)
            {
                return Controller.NotFound(entryEx.Message);
            }
            else if (ex is DuplicateValueException dupEx)
            {
                return Controller.Conflict(dupEx.Message);
            }
            else
            {
                return Controller.BadRequest(ex.Message);
            }
        }
    }

    public class WebServiceManager<TContext> : WebServiceManager where TContext : DbContext
    {
        TContext _dbContext { get; set; }

        public WebServiceManager(ControllerBase controller, TContext dbContext) : base(controller)
        {
            _dbContext = dbContext;
        }

        public IActionResult Perform(Func<TContext, IActionResult> callback)
        {
            if (!Controller.ModelState.IsValid)
            {
                return Controller.BadRequest(Controller.ModelState);
            }

            
                try
                {
                    return callback(_dbContext);
                }
                catch (Exception ex)
                {
                    return HandleException(ex);
                }
            
        }

        public IActionResult Perform(Action<TContext> callback)
        {
            if (!Controller.ModelState.IsValid)
            {
                return Controller.BadRequest(Controller.ModelState);
            }

            
                try
                {
                    callback(_dbContext);

                    return Controller.NoContent();
                }
                catch (Exception ex)
                {
                    return HandleException(ex);
                }
            
        }

        public async Task<IActionResult> Perform(Func<TContext, Task<IActionResult>> callback)
        {
            if (!Controller.ModelState.IsValid)
            {
                return Controller.BadRequest(Controller.ModelState);
            }

            
                try
                {
                    return await callback(_dbContext);
                }
                catch (Exception ex)
                {
                    return HandleException(ex);
                }
            
        }

        public async Task<IActionResult> Perform(Func<TContext, Task> callback)
        {
            if (!Controller.ModelState.IsValid)
            {
                return Controller.BadRequest(Controller.ModelState);
            }

            
                try
                {
                    await callback(_dbContext);

                    return Controller.NoContent();
                }
                catch (Exception ex)
                {
                    return HandleException(ex);
                }
            
        }
    }
}
