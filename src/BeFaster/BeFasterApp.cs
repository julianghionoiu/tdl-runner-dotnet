using BeFaster.Runner;
using BeFaster.Runner.Config;

namespace BeFaster
{
    internal class BeFasterApp
    {
        /// <summary>
        /// ~~~~~~~~~~ Running the system: ~~~~~~~~~~~~~
        /// </summary>
        /// <param name="args">Action.</param>
        private static void Main(string[] args)
        {
            ClientRunner.Build()
                .ForUsername(CredentialsConfig.Get("tdl_username"))
                .WithServerHostname(AppConfig.Hostname)
                .WithActionIfNoArgs(RunnerAction.Get(RunnerAction.Names.TestConnectivity))
                .Create()
                .Start(args);
        }
    }
}
