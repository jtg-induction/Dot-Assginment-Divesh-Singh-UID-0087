using RestaurantManagement.Constants;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace RestaurantManagement.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errors = actionContext.ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .ToDictionary(
                        // CLEANUP: Splits "adduser.Name" at the dot, takes "Name", 
                        // and turns it into clean front-end camelCase "name"
                        kvp => {
                            var rawKey = kvp.Key.Split('.').LastOrDefault() ?? kvp.Key;
                            return string.IsNullOrEmpty(rawKey)
                                ? rawKey
                                : char.ToLowerInvariant(rawKey[0]) + rawKey.Substring(1);
                        },
                        kvp => {
                            var firstError = kvp.Value.Errors.FirstOrDefault();
                            if (firstError == null) return string.Empty;

                            if (!string.IsNullOrEmpty(firstError.ErrorMessage))
                            {
                                return firstError.ErrorMessage;
                            }

                            if (firstError.Exception != null)
                            {
                                var rawKey = kvp.Key.Split('.').LastOrDefault() ?? ValidationMessages.Field;
                                return $"Provide a valid {rawKey} value";
                            }

                            return ValidationMessages.InvalidRequest;
                        }
                    );

                var errorPayload = new
                {
                    Success = false,
                    Message = ValidationMessages.ValidationError,
                    Errors = errors
                };

                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    errorPayload
                );
            }
        }
    }
}
