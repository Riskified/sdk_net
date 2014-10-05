using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model
{
    internal interface IJsonSerializable
    {
        void Validate(bool isWeak=false);
    }
}
