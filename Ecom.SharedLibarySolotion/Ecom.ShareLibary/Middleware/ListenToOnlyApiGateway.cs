using Microsoft.AspNetCore.Http;
namespace Ecom.ShareLibary.Middleware
{
    public class ListenToOnlyApiGateWay(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var signedHeader = context.Request.Headers["Api-Gateway"];

            // 503 status if signed is null
            if(signedHeader.FirstOrDefault() is null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Sorry, service is unavailable");
                return;
            }
            else
            {
                await next(context);
            }
        }
    }
}
