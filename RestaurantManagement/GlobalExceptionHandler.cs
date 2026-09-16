using RestaurantManagement.Constants;
using RestaurantManagement.Exceptions;
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
            if (exception is ResourceException)
            {
                var conflictResponse = context.Request.CreateResponse(
                    HttpStatusCode.Conflict,
                    new { Message = exception.Message } // Automatically uses the exact text you threw
                );
                context.Result = new ResponseMessageResult(conflictResponse);
                return;
            }
            // 2. Identify specific custom domain business logic exceptions
            if (exception is InvalidOperationException)
            {
                var badRequestResponse = context.Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    new { Message = exception.Message }
                );
                context.Result = new ResponseMessageResult(badRequestResponse);
                return;
            }

            // 3. Fallback for all unexpected database or system crashes (500 Internal Server Error)
            var genericResponse = context.Request.CreateResponse(
                HttpStatusCode.InternalServerError,
                new { Message =ValidationMessages.InternalServerError}
            );
            context.Result = new ResponseMessageResult(genericResponse);
        }
    }
}
