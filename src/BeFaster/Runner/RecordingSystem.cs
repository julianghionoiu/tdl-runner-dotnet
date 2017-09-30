using System;
using System.Net;
using BeFaster.Runner.Config;
using BeFaster.Runner.Exceptions;
using RestSharp;

namespace BeFaster.Runner
{
    internal static class RecordingSystem
    {
        private static readonly IRestClient RestClient = new RestClient(AppConfig.RecordingSystemEndpoint);

        public static bool IsRunning()
        {
            try
            {
                var request = new RestRequest("status", Method.GET);
                var response = RestClient.Execute(request);

                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                throw new RecordingSystemNotReachable(e);
            }
        }

        public static void NotifyEvent(string lastFetchedRound, string actionName)
        {
            Console.WriteLine($@"Notify round ""{lastFetchedRound}"", event ""{actionName}""");

            try
            {
                var request = new RestRequest("notify", Method.POST);
                request.AddBody($"{lastFetchedRound}/{actionName}");
                var response = RestClient.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine($"Recording system returned code: {response.StatusCode}");
                }
                else if (!response.Content.StartsWith("ACK"))
                {
                    Console.WriteLine($"Recording system returned body: {response.Content}");
                }
            }
            catch (Exception e)
            {
                throw new RecordingSystemNotReachable(e);
            }
        }
    }
}
