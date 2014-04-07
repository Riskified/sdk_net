using System;
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
            var gateway = new RiskifiedGateway("127.0.0.1","","");
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
