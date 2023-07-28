using TiVi.AuthService.Models.Base;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace TiVi.AuthService.Utilities.GlobalErrorHandling
{
    public class ActionFilterException : IActionFilter, IExceptionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            CheckModelState(context);
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnException(ExceptionContext context)
        {
            //DoLog(context, "FAILED");
            HandleException(context);

        }

        private void CheckModelState(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.HttpContext.Response.ContentType = "application/json";

                BaseJsonResponseError baseJsonResponseError;

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                BaseJsonResponse<object> baseJsonResponse = new BaseJsonResponse<object>();

                foreach (var state in context.ModelState)
                {
                    if (state.Value.ValidationState == ModelValidationState.Invalid)
                    {
                        baseJsonResponseError = new BaseJsonResponseError("Invalid parameter", state.Value.Errors[0].ErrorMessage);
                        baseJsonResponse.errors.Add(baseJsonResponseError);
                    }
                }

                context.Result = new JsonResult(baseJsonResponse);
            }
        }

        private void HandleException(ExceptionContext context)
        {
            if (context.Exception != null)
            {
                if (context.Exception.Data.Count > 0)
                {
                    context.HttpContext.Response.ContentType = "application/json";

                    BaseJsonResponseError baseJsonResponseError;

                    context.HttpContext.Response.StatusCode = context.Exception.Data["StatusCode"] != null ? (int)context.Exception.Data["StatusCode"] : (int)HttpStatusCode.InternalServerError;

                    string? cause = context.Exception.Data["Cause"] != null ? context.Exception.Data["Cause"]?.ToString() : context.Exception.Message; // kalau kosong akan diisi sama dengan message karena ada bberapa modul memakai ini
                    string? message = context.Exception.Data["Message"] != null ? context.Exception.Data["Message"]?.ToString() : "";

                    baseJsonResponseError = new BaseJsonResponseError(message, cause);

                    BaseJsonResponse<object> baseJsonResponse = new BaseJsonResponse<object>();

                    baseJsonResponse.errors.Add(baseJsonResponseError);

                    baseJsonResponse.data = new { TraceId = context.HttpContext.TraceIdentifier };



                    context.Result = new JsonResult(baseJsonResponse);

                    context.ExceptionHandled = true;

                    // logging 
                    //NLog.Logger logger = NLog.LogManager.GetLogger("ApiLog");
                    //logger.Error(context.Exception, context.Exception.Message);
                    // logging end
                }
            }
        }
    }
}
