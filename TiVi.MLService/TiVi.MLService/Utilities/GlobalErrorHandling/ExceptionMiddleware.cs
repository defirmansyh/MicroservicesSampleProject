using TiVi.MLService.Models.Base;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net;

namespace TiVi.MLService.Utilities.GlobalErrorHandling
{
    public static class ExceptionMiddleware
    {
        public static void ConfigureExceptionHandlerWithTraceId(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        context.Response.ContentType = "application/json";

                        BaseJsonResponseError baseJsonResponseError;

                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var message = contextFeature.Error.GetBaseException().Message;

                        baseJsonResponseError = new BaseJsonResponseError("Internal Server Error", message);

                        BaseJsonResponse<object> baseJsonResponse = new BaseJsonResponse<object>();

                        baseJsonResponse.errors.Add(baseJsonResponseError);

                        baseJsonResponse.data = null;//new { TraceId = context.TraceIdentifier };

                        var jsonString = JsonConvert.SerializeObject(baseJsonResponse, new Newtonsoft.Json.JsonSerializerSettings
                        {
                            ContractResolver = new DefaultContractResolver
                            {
                                NamingStrategy = new SnakeCaseNamingStrategy()
                            }
                        });

                        await context.Response.WriteAsync(jsonString);
                    }
                });
            });
        }
    }

}
