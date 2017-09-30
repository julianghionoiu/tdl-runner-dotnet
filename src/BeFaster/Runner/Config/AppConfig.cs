using System.Configuration;

namespace BeFaster.Runner.Config
{
    internal static class AppConfig
    {
        public static string Hostname => ConfigurationManager.AppSettings[nameof(Hostname)];

        public static string RecordingSystemEndpoint => ConfigurationManager.AppSettings[nameof(RecordingSystemEndpoint)];
    }
}
