using System.Net;
using System.Text.Json;

namespace ItemComparison.Api.Middleware;

public sealed class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (Exception ex)
        {
            ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            ctx.Response.ContentType = "application/json";
            var payload = new { error = "Unexpected error", detail = ex.Message, traceId = ctx.TraceIdentifier };
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}
