using Application.Exceptions;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        private readonly ILogService _logService;

        public ErrorsController(MongoLogService logService)
        {
            _logService = logService;
        }

        [Route("error")]
        public TomCarException Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;
            int code = (int)HttpStatusCode.InternalServerError;

            if (exception is ValidationException) code = (int)HttpStatusCode.BadRequest;
            else if (exception is RecordNotFoundException) code = (int)HttpStatusCode.NotFound;

            Response.StatusCode = code;

            var user = User.FindFirst(ClaimTypes.Email);
            var userEmail = user != null ? user.Value : null;

            _logService.Error(exception.Message, userEmail, exception.StackTrace, code.ToString());

            return new TomCarException(exception);
        }
    }
}