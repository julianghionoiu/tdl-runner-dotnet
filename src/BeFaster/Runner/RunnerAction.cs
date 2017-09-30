using System;
using System.Linq;
using TDL.Client.Actions;

namespace BeFaster.Runner
{
    internal class RunnerAction
    {
        public static class Names
        {
            public const string GetNewRoundDescription = "new";
            public const string TestConnectivity = "test";
            public const string DeployToProduction = "deploy";
        }

        private static readonly RunnerAction[] Actions =
        {
            new RunnerAction(Names.GetNewRoundDescription, ClientActions.Stop),
            new RunnerAction(Names.TestConnectivity, ClientActions.Stop),
            new RunnerAction(Names.DeployToProduction, ClientActions.Publish)
        };

        public static RunnerAction Get(string name) => Actions.FirstOrDefault(a => a.ShortName.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        public string ShortName { get; }
        public IClientAction ClientAction { get; }

        private RunnerAction(string shortName, IClientAction clientAction)
        {
            ShortName = shortName;
            ClientAction = clientAction;
        }
    }
}
