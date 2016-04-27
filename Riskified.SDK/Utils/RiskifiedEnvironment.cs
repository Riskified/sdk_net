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

    public static class EnvironmentsUrls 
    {
        private static readonly Dictionary<RiskifiedEnvironment, string> EnvToUrl;

        static EnvironmentsUrls()
        {
            EnvToUrl = new Dictionary<RiskifiedEnvironment, string>(4);
            
            if(!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DebugRiskifiedHostUrl"]))
                EnvToUrl.Add(RiskifiedEnvironment.Debug, ConfigurationManager.AppSettings["DebugRiskifiedHostUrl"]);
            EnvToUrl.Add(RiskifiedEnvironment.Sandbox, "https://sandbox.riskified.com");
            EnvToUrl.Add(RiskifiedEnvironment.Staging, "https://s.riskified.com");
            EnvToUrl.Add(RiskifiedEnvironment.Production, "https://wh.riskified.com");
        }

        public static string GetEnvUrl(RiskifiedEnvironment env)
        {
            if (EnvToUrl.ContainsKey(env))
                return EnvToUrl[env];
            
            throw new RiskifiedException(string.Format("Riskified environment '{0}' doesn't exist", env));
        }
    }
}
