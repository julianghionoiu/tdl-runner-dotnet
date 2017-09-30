﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using BeFaster.Runner.Exceptions;
using BeFaster.Runner.Extensions;
using BeFaster.Runner.Utils;

namespace BeFaster.Runner.Config
{
    internal static class CredentialsConfig
    {
        private static readonly Dictionary<string, string> Properties = new Dictionary<string, string>();

        static CredentialsConfig()
        {
            try
            {
                foreach (var row in File.ReadAllLines(AppConfig.CredentialsPath))
                {
                    var data = row.Split('=');
                    Properties.Add(data[0], string.Join("=", data.Skip(1)));
                }
            }
            catch (IOException e)
            {
                throw new ConfigNotFoundException($@"The ""credentials.config"" has not been found. Please download from challenge page. ( Reason: ""{e.Message}"")", e);
            }
        }

        public static string Get(string key) =>
            Optional<string>
                .OfNullable(Properties.GetValueOrDefault(key))
                .OrElseThrow(() => new ConfigNotFoundException($@"The ""credentials.config"" file does not contain key {key}"));

        public static string Get(string key, string defaultValue) =>
            Optional<string>
                .OfNullable(Properties.GetValueOrDefault(key))
                .OrElse(defaultValue);
    }
}
