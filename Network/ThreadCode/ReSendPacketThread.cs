using AGV_V1._0.Network.ThreadCode;
using AGV_V1._0.NLog;
using AGV_V1._0.Queue;
using AGV_V1._0.Util;
using AGVSocket.Network;
using AGVSocket.Network.Packet;
using System;
using System.Threading;

namespace AGV_V1._0.ThreadCode
{
    class ReSendPacketThread:BaseThread
    {
         private static ReSendPacketThread instance;
        public static ReSendPacketThread Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ReSendPacketThread();
                }
                return instance;
            }
        }
        private ReSendPacketThread() { }

        protected override string ThreadName()
        {
            return "ReSendPacketThread";
        }
        protected override void Run()
        {
            try
            {
                if (ReSendPacketQueue.Instance.IsHasData())
                {
                    Vehicle v = ReSendPacketQueue.Instance.Dequeue();
                    AgvServerManager.Instance.SendTo(v.LastSendPacket, v.Id);
                    Logs.Warn(v.Id + " resend");
                }
                Thread.Sleep(ConstDefine.RESEND_TIME);
            }
            catch (Exception e)
            {
                Logs.Error("ReSendpacketThread" + e);
            }
        }
    }
}
