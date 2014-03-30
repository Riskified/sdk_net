using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Riskified.NetSDK.Control;

namespace Riskified.Tests.Control.Tests
{
    [TestFixture]
    class RiskifiedGatewayTests
    {


        [Test]
        public void CreateOrUpdateOrder_ValidOrderValidLink_SendsDataCorrectly()
        {
            #region setup
            RiskifiedGateway gateway = new RiskifiedGateway(new Uri("127.0.0.1"),"","");
            #endregion

            #region act
            //gateway
            #endregion

            #region verify

            #endregion
        }

        [Test]
        public void CreateOrUpdateOrder_ValidOrderBrokenLink_ThrowsException()
        {
            
        }

        [Test]
        public void CreateOrUpdateOrder_MissingOrderFieldsValidLink_ThrowsException()
        {
            
        }
    }
}
