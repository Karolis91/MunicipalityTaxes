namespace MunicipalityTaxes.Middleware
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using Exceptions;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case FileNotFoundException fileNotFoundException:
                        context.Response.Clear();
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject("File not found."));
                        break;
                    case FileEmptyException fileEmptyException:
                        context.Response.Clear();
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject("File is empty."));
                        break;
                    default:
                        context.Response.Clear();
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject("Something went wrong."));
                        break;
                }
            }
        }
    }
}