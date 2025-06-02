using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Json;

namespace UserService.API.DI;

public static class LoggingConfiguration
{
    public static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Console()
            .WriteTo.Http(
                requestUri: configuration["Logstash:Uri"],
                queueLimitBytes: null,
                textFormatter: new JsonFormatter())
            .CreateLogger();
        return services;
    }
}
