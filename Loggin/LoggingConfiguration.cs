using Serilog;
using Serilog.Events;

namespace Bingie.Loggin;

public static class LoggingConfiguration
{
    public static void ConfigureLogging()
    {
        var logDirectory = Path.Combine(FileSystem.AppDataDirectory, "logs");

        // Ensure the logs directory exists
        if (!Directory.Exists(logDirectory)) _ = Directory.CreateDirectory(logDirectory);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.File(Path.Combine(logDirectory, "BingieLogR-.log"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7)
            .CreateLogger();
    }
}