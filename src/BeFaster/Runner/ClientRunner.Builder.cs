namespace BeFaster.Runner
{
    internal partial class ClientRunner
    {
        public class Builder
        {
            private string username;
            private string hostname;
            private RunnerAction defaultRunnerAction;

            public Builder ForUsername(string username)
            {
                this.username = username;
                return this;
            }

            public Builder WithServerHostname(string hostname)
            {
                this.hostname = hostname;
                return this;
            }

            public Builder WithActionIfNoArgs(RunnerAction defaultRunnerAction)
            {
                this.defaultRunnerAction = defaultRunnerAction;
                return this;
            }

            public ClientRunner Create() => new ClientRunner(username, hostname, defaultRunnerAction);
        }
    }
}
