using System;
using System.IO;
using System.Linq;
using System.Text;

namespace BeFaster.Runner
{
    internal static class RoundManagement
    {
        private static readonly string ChallengesPath;
        private static readonly string LastFetchedRoundPath;

        static RoundManagement()
        {
            var srcPath = GetParent(Environment.CurrentDirectory, "src");
            var repoPath = Directory.GetParent(srcPath).FullName;

            ChallengesPath = Path.Combine(repoPath, "challenges");
            LastFetchedRoundPath = Path.Combine(ChallengesPath, "XR.txt");
        }

        public static string DisplayAndSaveDescription(string label, string description)
        {
            Console.WriteLine($"Starting round: {label}");
            Console.WriteLine(description);

            // Save description.
            var descriptionPath = Path.Combine(ChallengesPath, $"{label}.txt");

            File.WriteAllText(descriptionPath, description.Replace("\n", Environment.NewLine));
            Console.WriteLine($"Challenge description saved to file: {descriptionPath}.");

            // Save round label.
            File.WriteAllText(LastFetchedRoundPath, label);

            return "OK";
        }

        public static string GetLastFetchedRound() =>
            File.Exists(LastFetchedRoundPath)
                ? File.ReadLines(LastFetchedRoundPath, Encoding.Default).FirstOrDefault()
                : "noRound";

        private static string GetParent(string path, string parentName)
        {
            while (true)
            {
                var directory = new DirectoryInfo(path);

                if (directory.Parent == null)
                {
                    return null;
                }

                if (directory.Parent.Name == parentName)
                {
                    return directory.Parent.FullName;
                }

                path = directory.Parent.FullName;
            }
        }
    }
}
