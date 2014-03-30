using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.NetSDK.Model
{
    struct OrderTransactionResult
    {
        /// <summary>
        /// A flag that signs if the transaction was finished successfully
        /// Values of @OrderID and @ErrorMessage will be set accordingly
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// A unique order ID received from the Riskified server for later submittion or notification regarding that specific order
        /// Will be 0 in case of unsuccessful transaction (IsSuccess==false)
        /// TODO should it be int/string/long etc.?!
        /// </summary>
        public int OrderId { get; private set; }
        
        /// <summary>
        /// Error Message that describes the error that occured during an Order Transaction with the server
        /// Will be null or empty in case of successful transaction (IsSuccess==true) 
        /// </summary>
        public string ErrorMessage { get; private set; }

            public OrderTransactionResult(int orderId, string errorMessage) : this()
        {
            if ((orderId == 0 && string.IsNullOrEmpty(errorMessage))
                || (orderId != 0 && !string.IsNullOrEmpty(errorMessage)))
                throw new ArgumentException(
                    "Arguments combination is invalid. Only 1 of orderId and errorMessage should be set to an actual valid value");
                
            OrderId = orderId;
            ErrorMessage = errorMessage;
            if (orderId == 0)
                IsSuccess = false;
        }
    }
}
