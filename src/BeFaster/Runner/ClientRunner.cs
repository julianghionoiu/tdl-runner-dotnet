using System;
using System.Collections.Generic;
using System.Linq;
using BeFaster.Runner.Extensions;
using BeFaster.Runner.Utils;
using BeFaster.Solutions;
using TDL.Client;
using TDL.Client.Actions;

namespace BeFaster.Runner
{
    internal partial class ClientRunner
    {
        private readonly string username;
        private readonly string hostname;
        private readonly RunnerAction defaultRunnerAction;

        private ClientRunner(
            string username,
            string hostname,
            RunnerAction defaultRunnerAction)
        {
            this.username = username;
            this.hostname = hostname;
            this.defaultRunnerAction = defaultRunnerAction;
        }

        public static Builder Build() => new Builder();

        public void Start(string[] args)
        {
            if (!IsRecordingSystemOk())
            {
                Console.WriteLine("Please run `record_screen_and_upload` before continuing.");
                return;
            }

            var runnerAction = ExtractActionFrom(args).OrElse(defaultRunnerAction);
            Console.WriteLine("Chosen action is: " + runnerAction.ShortName);

            var client = TdlClient.Build()
                .SetHostname(hostname)
                .SetUniqueId(username)
                .Create();

            var processingRules = new ProcessingRules();
            processingRules
                .On("display_description")
                .Call(p => RoundManagement.DisplayAndSaveDescription(p[0], p[1]))
                .Then(ClientActions.Publish);

            processingRules
                .On("sum")
                .Call(p => SumSolution.Sum(p[0].AsInt(), p[1].AsInt()))
                .Then(runnerAction.ClientAction);

            processingRules
                .On("hello")
                .Call(p => HelloSolution.Hello(p[0]))
                .Then(runnerAction.ClientAction);

            processingRules
                .On("fizz_buzz")
                .Call(p => FizzBuzzSolution.FizzBuzz(p[0].AsInt()))
                .Then(runnerAction.ClientAction);

            processingRules
                .On("checkout")
                .Call(p => CheckoutSolution.Checkout(p[0]))
                .Then(runnerAction.ClientAction);

            client.GoLiveWith(processingRules);
        }

        private static Maybe<RunnerAction> ExtractActionFrom(IEnumerable<string> args)
        {
            var actionName = args.FirstOrDefault() ?? string.Empty;
            var action = RunnerAction.Get(actionName);
            return action != null
                ? Maybe<RunnerAction>.Some(action)
                : Maybe<RunnerAction>.None;
        }

        private bool IsRecordingSystemOk()
        {
            throw new System.NotImplementedException();
        }
    }
}
