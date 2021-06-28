using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Test.Services;

namespace Test
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IIdentityService _service;

        public AuthenticationMiddleware(RequestDelegate next, IIdentityService service)
        {
            _next = next;
            _service = service;
        }

        public async Task Invoke(HttpContext context)
        {
            string header = context.Request.Headers["Authorization"];
            
            if (header is not null && header == _service.GetToken(_service.GetIdentity("admin" , "admin_password")))
            {
                await _next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = 401;
            }
        }
    }
}