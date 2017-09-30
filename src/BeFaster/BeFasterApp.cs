using BeFaster.Runner;
using BeFaster.Runner.Config;

namespace BeFaster
{
    internal class BeFasterApp
    {
        /// <summary>
        /// ~~~~~~~~~~ Running the system: ~~~~~~~~~~~~~
        /// 
        ///   From IDE:
        ///      In project properties set the value of Debug/Start Options/Command Line Arguments
        ///      Run this project from the IDE.
        ///
        ///   Available actions:
        ///        * getNewRoundDescription    - Get the round description (call once per round).
        ///        * testConnectivity          - Test you can connect to the server (call any number of time)
        ///        * deployToProduction        - Release your code. Real requests will be used to test your solution.
        ///                                      If your solution is wrong you get a penalty of 10 minutes.
        ///                                      After you fix the problem, you should deploy a new version into production.
        ///
        /// ~~~~~~~~~~ The workflow ~~~~~~~~~~~~~
        ///
        ///   +------+-----------------------------------------+-----------------------------------------------+
        ///   | Step |          IDE                            |         Web console                           |
        ///   +------+-----------------------------------------+-----------------------------------------------+
        ///   |  1.  |                                         | Start a challenge, should display "Started"   |
        ///   |  2.  | Run "getNewRoundDescription"            |                                               |
        ///   |  3.  | Read description from ./challenges      |                                               |
        ///   |  4.  | Implement the required method in        |                                               |
        ///   |      |   ./src/BeFaster/Solutions              |                                               |
        ///   |  5.  | Run "testConnectivity", observe output  |                                               |
        ///   |  6.  | If ready, run "deployToProduction"      |                                               |
        ///   |  7.  |                                         | Type "done"                                   |
        ///   |  8.  |                                         | Check failed requests                         |
        ///   |  9.  |                                         | Go to step 2.                                 |
        ///   +------+-----------------------------------------+-----------------------------------------------+
        /// 
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
