using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGV_V1._0.Network.Server.APM
{
    class AgvIdSessionKeyBuilder : ISessionKeyBuilder
    {
        public string GetSessionKey(string remoteEndPoint)
        {
            string ip = remoteEndPoint.Split(':')[0];
            string[] nums = ip.Split('.');
            string key = nums[nums.Length - 1];
            return key;
           
        }
    }
}
