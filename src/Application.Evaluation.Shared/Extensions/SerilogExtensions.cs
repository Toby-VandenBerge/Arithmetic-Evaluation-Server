using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Arithmetic.Evaluation.Shared.Extensions
{
    public static class SerilogExtensions
    {
        private static bool initialized = false;

        public static void InitializeSerilog(IConfiguration configuration)
        {
            if (initialized)
            {
                return;
            }

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug,
                    theme: AnsiConsoleTheme.Code,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}")
                .CreateLogger();

            initialized = true;
        }
    }
}
