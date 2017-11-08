using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Arithmetic.Evaluation.Shared.Extensions
{
    public static class SerilogExtensions
    {
        private static bool initialized = false;

        /// <summary>
        /// Initialize the static Serilog Log.
        /// </summary>
        /// <remarks>Can only be called once</remarks>
        /// <param name="configuration"></param>
        public static void InitializeSerilog(IConfiguration configuration)
        {
            // Ensure the Logger is only created once.
            if (initialized)
            {
                return;
            }

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug,
                    theme: AnsiConsoleTheme.Code,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] [{SourceContext}] [{ThreadId}] {Message}{NewLine}{Exception}")
                .CreateLogger();

            initialized = true;
        }
    }
}
