using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace Main.Middleware;

public class TokenMiddleware
{
    private readonly RequestDelegate _next;

    public TokenMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
            await _next.Invoke(context);
        //var token = context.Request.Headers["Authorization"];
        //if (token.Count > 0)
        //{
        //    //SetClaims(token[0]);
        //    await _next.Invoke(context);
        //}
        //else
        //{
        //    context.Response.StatusCode = 403;
        //    await context.Response.WriteAsync("Token is invalid");
        //}
    }

    private void SetClaims(string token)
    {

    }
}

