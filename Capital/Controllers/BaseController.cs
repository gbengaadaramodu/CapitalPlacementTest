using Core.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Capital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        [NonAction]
        public IActionResult PrepareResponse(HttpStatusCode statusCode, string message, bool error, object content)
        {
            return StatusCode((int)statusCode, new ApiResponse(message, statusCode, content, error));
        }
        
        [NonAction]
        public IActionResult PrepareResponse(HttpStatusCode statusCode, string message, bool error)
        {
            return StatusCode((int)statusCode, new ApiResponse(message, statusCode, error));
        }

        [NonAction]
        public IActionResult PrepareResponse(HttpStatusCode statusCode, string message, bool error, object content, int totalSize)
        {
            return StatusCode((int)statusCode, new ApiResponse(message, statusCode, content, error, totalSize));
        }

        public AppUser CurrentUser
        {
            get
            {
                return new AppUser(this.User as ClaimsPrincipal);
            }
        }


        [NonAction]
        public string getAction()
        {
            return ControllerContext.ActionDescriptor.ActionName;
        }
        [NonAction]
        public string getController()
        {
            return ControllerContext.ActionDescriptor.ControllerName;
        }
    }
}
