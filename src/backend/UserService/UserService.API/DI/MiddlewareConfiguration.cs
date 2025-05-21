using UserService.API.Middlewares;

namespace UserService.API.DI;

public static class MiddlewareConfiguration
{
    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }
}