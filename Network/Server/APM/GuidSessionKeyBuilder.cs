using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGV_V1._0.Network.Server.APM
{
    class GuidSessionKeyBuilder : ISessionKeyBuilder
    {
        public string GetSessionKey(string remoteEndPoint)
        {
           return Guid.NewGuid().ToString();
        }
    }
}
