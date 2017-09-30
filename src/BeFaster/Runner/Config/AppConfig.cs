using System.Configuration;

namespace BeFaster.Runner.Config
{
    internal static class AppConfig
    {
        public static string Hostname => ConfigurationManager.AppSettings[nameof(Hostname)];
        public static string RecordingSystemEndpoint => ConfigurationManager.AppSettings[nameof(RecordingSystemEndpoint)];
        public static string CredentialsFilePath => ConfigurationManager.AppSettings[nameof(CredentialsFilePath)];
        public static string ChallengesFolderPath => ConfigurationManager.AppSettings[nameof(ChallengesFolderPath)];
        public static string LastFetchedRoundFileName => ConfigurationManager.AppSettings[nameof(LastFetchedRoundFileName)];
    }
}
