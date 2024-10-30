using Microsoft.AspNetCore.Http;
using System.Text;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public ExceptionMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // Get the endpoint details
                var request = httpContext.Request;
                var method = request.Method;
                var path = request.Path;

                // Call the method to handle the exception and return a response
                await HandleExceptionAsync(httpContext, ex, method, path);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, string method, string path)
        {

            var endpointSB = new StringBuilder();
            endpointSB.Append(method);
            endpointSB.Append("/");
            endpointSB.Append(path);
            var endpoint = endpointSB.ToString();

            var response = new SystemErrorLog(exception.Message, endpoint, context.Response.StatusCode);


            using (var scope = _serviceProvider.CreateScope())
            {
                var logService = scope.ServiceProvider.GetRequiredService<ILogService>();
                _ = logService.Log(response);
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
           
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
