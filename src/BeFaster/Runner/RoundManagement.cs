using System;
using System.IO;
using System.Linq;
using System.Text;

namespace BeFaster.Runner
{
    internal static class RoundManagement
    {
        private static readonly string LastFetchedRoundPath = Path.GetFullPath(@"..\..\..\..\challenges\XR.txt");

        public static string DisplayAndSaveDescription(string label, string description)
        {
            Console.WriteLine($"Starting round: {label}");
            Console.WriteLine(description);

            // Save description.
            var descriptionPath = Path.GetFullPath($@"..\..\..\..\challenges\{label}.txt");
            File.WriteAllText(descriptionPath, description.Replace("\n", Environment.NewLine));
            Console.WriteLine($"Challenge description saved to file: {descriptionPath}.");

            // Save round label.
            File.WriteAllText(LastFetchedRoundPath, label);

            return "OK";
        }

        public static string GetLastFetchedRound() =>
            File.ReadLines(LastFetchedRoundPath, Encoding.Default).FirstOrDefault()
            ?? "noRound";
    }
}
