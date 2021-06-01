using Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        [Route("error")]
        public TomCarException Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;
            int code = (int)HttpStatusCode.InternalServerError;

            if (exception is ValidationException) code = (int)HttpStatusCode.BadRequest;
            else if (exception is RecordNotFoundException) code = (int)HttpStatusCode.NotFound;

            Response.StatusCode = code;

            return new TomCarException(exception);
        }
    }
}