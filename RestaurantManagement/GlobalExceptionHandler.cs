using RestaurantManagement.Constants;
using RestaurantManagement.Exceptions;
using RestaurantManagement.Services.Exceptions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace RestaurantManagement.Handlers
{
    /// <summary>
    /// Centralized exception handler that intercepts unhandled exceptions 
    /// across the application to format clean API error responses.
    /// </summary>
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
           
            var exception = context.Exception;
            var errorPayload = new
            {
                Success = false,
                Message = exception.Message
            };
            if (exception is NotFoundException)
            {

                var unauthorizedResponse = context.Request.CreateResponse(
                    HttpStatusCode.NotFound,
                   errorPayload
                );
                context.Result = new ResponseMessageResult(unauthorizedResponse);
                return;
            }
            // 1. Intercept service-level authentication exceptions
            if (exception is UnauthenticatedException)
            {
              
                var unauthorizedResponse = context.Request.CreateResponse(
                    HttpStatusCode.Unauthorized,
                   errorPayload
                );
                context.Result = new ResponseMessageResult(unauthorizedResponse);
                return;
            }

            // 2. Identify resource conflicts
            if (exception is ResourceException)
            {
               
                var conflictResponse = context.Request.CreateResponse(
                    HttpStatusCode.Conflict,
                   errorPayload
                );
                context.Result = new ResponseMessageResult(conflictResponse);
                return;
            }

            // 3. Identify specific custom domain business logic exceptions
            if (exception is InvalidOperationException)
            {
               
                var badRequestResponse = context.Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                   errorPayload
                );
                context.Result = new ResponseMessageResult(badRequestResponse);
                return;
            }

            // 4. Fallback for all unexpected database or system crashes (500 Internal Server Error)

            var genericResponse = context.Request.CreateResponse(
                HttpStatusCode.InternalServerError,
                errorPayload
            );
            context.Result = new ResponseMessageResult(genericResponse);
        }
    }
}
