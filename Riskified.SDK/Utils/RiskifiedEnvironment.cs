using System.Collections.Generic;
using Riskified.SDK.Exceptions;
using System.Configuration;

namespace Riskified.SDK.Utils
{
    public enum RiskifiedEnvironment
    {
        Debug,
        Sandbox,
        Staging,
        Production
    }

    public enum FlowStrategy
    {
        Default,
        Sync,
        Account
    }

    internal static class EnvironmentsUrls 
    {
        private static readonly Dictionary<RiskifiedEnvironment, Dictionary<FlowStrategy, string>> EnvToUrl;

        private static readonly Dictionary<FlowStrategy, string> DebugUrl;
        private static readonly Dictionary<FlowStrategy, string> SandboxUrl;
        private static readonly Dictionary<FlowStrategy, string> StagingUrl;
        private static readonly Dictionary<FlowStrategy, string> ProductionUrl;

        static EnvironmentsUrls()
        {
            EnvToUrl = new Dictionary<RiskifiedEnvironment, Dictionary<FlowStrategy, string>>(4);

            DebugUrl = new Dictionary<FlowStrategy, string>(2);
            SandboxUrl = new Dictionary<FlowStrategy, string>(2);
            StagingUrl = new Dictionary<FlowStrategy, string>(2);
            ProductionUrl = new Dictionary<FlowStrategy, string>(3);

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DebugRiskifiedHostUrl"]))
                DebugUrl.Add(FlowStrategy.Default, ConfigurationManager.AppSettings["DebugRiskifiedHostUrl"]);
                EnvToUrl.Add(RiskifiedEnvironment.Debug, DebugUrl);

            SandboxUrl.Add(FlowStrategy.Default, "https://sandbox.riskified.com");
            SandboxUrl.Add(FlowStrategy.Account, "https://api-sandbox.riskified.com");
            EnvToUrl.Add(RiskifiedEnvironment.Sandbox, SandboxUrl);

            StagingUrl.Add(FlowStrategy.Default, "https://s.riskified.com");
            EnvToUrl.Add(RiskifiedEnvironment.Staging, StagingUrl);

            ProductionUrl.Add(FlowStrategy.Default, "https://wh.riskified.com");
            ProductionUrl.Add(FlowStrategy.Sync, "https://wh-sync.riskified.com");
            ProductionUrl.Add(FlowStrategy.Account, "https://api.riskified.com");
            EnvToUrl.Add(RiskifiedEnvironment.Production, ProductionUrl);
        }

        public static Dictionary<FlowStrategy, string> GetEnv(RiskifiedEnvironment env)
        {
            if (EnvToUrl.ContainsKey(env))
                return EnvToUrl[env];
            
            throw new RiskifiedException(string.Format("Riskified environment '{0}' doesn't exist", env));
        }

        public static string GetEnvUrl(RiskifiedEnvironment env, FlowStrategy flow)
        {
            var CurrentEnv = GetEnv(env);
            return CurrentEnv.ContainsKey(flow) ? CurrentEnv[flow] : CurrentEnv[FlowStrategy.Default];
        }
    }
}
