using TDL.Client.Actions;

namespace BeFaster.Runner
{
    public class RunnerAction
    {
        public string ShortName { get; }
        public IClientAction ClientAction { get; }

        private RunnerAction(string shortName, IClientAction clientAction)
        {
            ShortName = shortName;
            ClientAction = clientAction;
        }

        public static readonly RunnerAction GetNewRoundDescription = new RunnerAction("new", ClientActions.Stop);
        public static readonly RunnerAction TestConnectivity = new RunnerAction("test", ClientActions.Stop);
        public static readonly RunnerAction DeployToProduction = new RunnerAction("deploy", ClientActions.Publish);

        public static readonly RunnerAction[] AllActions =
        {
            GetNewRoundDescription,
            TestConnectivity,
            DeployToProduction
        };
    }
}
