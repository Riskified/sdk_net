using System.Collections.Generic;
using Riskified.SDK.Exceptions;
using System.Configuration;

namespace Riskified.SDK.Utils
{
    public enum RiskifiedEnvironment
    {
        Debug,
        Sandbox,
        Production
    }

    public enum FlowStrategy
    {
        Default,
        Sync,
        Account,
        Otp,
        Deco
    }

    internal static class EnvironmentsUrls 
    {
        private static readonly Dictionary<RiskifiedEnvironment, Dictionary<FlowStrategy, string>> EnvToUrl;

        private static readonly Dictionary<FlowStrategy, string> DebugUrl;
        private static readonly Dictionary<FlowStrategy, string> SandboxUrl;
        private static readonly Dictionary<FlowStrategy, string> ProductionUrl;

        static EnvironmentsUrls()
        {
            EnvToUrl = new Dictionary<RiskifiedEnvironment, Dictionary<FlowStrategy, string>>(3);

            DebugUrl = new Dictionary<FlowStrategy, string>(1);
            SandboxUrl = new Dictionary<FlowStrategy, string>(3);
            ProductionUrl = new Dictionary<FlowStrategy, string>(4);

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DebugRiskifiedHostUrl"]))
                DebugUrl.Add(FlowStrategy.Default, ConfigurationManager.AppSettings["DebugRiskifiedHostUrl"]);
                EnvToUrl.Add(RiskifiedEnvironment.Debug, DebugUrl);

            SandboxUrl.Add(FlowStrategy.Default, "https://sandbox.riskified.com");
            SandboxUrl.Add(FlowStrategy.Account, "https://api-sandbox.riskified.com");
            SandboxUrl.Add(FlowStrategy.Deco, "https://sandboxw.decopayments.com");
            SandboxUrl.Add(FlowStrategy.Otp, "https://otp-sandbox.self-veri.com");
            EnvToUrl.Add(RiskifiedEnvironment.Sandbox, SandboxUrl);

            ProductionUrl.Add(FlowStrategy.Default, "https://wh.riskified.com");
            ProductionUrl.Add(FlowStrategy.Sync, "https://wh-sync.riskified.com");
            ProductionUrl.Add(FlowStrategy.Account, "https://api.riskified.com");
            ProductionUrl.Add(FlowStrategy.Deco, "https://w.decopayments.com");
            ProductionUrl.Add(FlowStrategy.Otp, "https://otp.self-veri.com/recover/v1/otp");
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
