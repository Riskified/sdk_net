using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    internal static class EnvironmentsUrls 
    {
        private static Dictionary<RiskifiedEnvironment, string> _envToUrl;

        static EnvironmentsUrls()
        {
            _envToUrl = new Dictionary<RiskifiedEnvironment, string>(4);
            
            if(!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DebugRiskifiedHostUrl"]))
                _envToUrl.Add(RiskifiedEnvironment.Debug, "localhost:3000");
            _envToUrl.Add(RiskifiedEnvironment.Sandbox, "sandbox.riskified.com");
            _envToUrl.Add(RiskifiedEnvironment.Staging, "s.riskified.com");
            _envToUrl.Add(RiskifiedEnvironment.Production, "wh.riskified.com");
        }

        public static string GetEnvUrl(RiskifiedEnvironment env)
        {
            if (_envToUrl.ContainsKey(env))
                return _envToUrl[env];
            
            throw new RiskifiedException(string.Format("Riskified environment '{0}' doesn't exist", env));
        }
    }
}
