using AGV_V1._0.Network.ThreadCode;
using AGV_V1._0.NLog;
using System;

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
                //if (ReSendPacketQueue.Instance.IsHasData())
                //{
                //    SendBasePacket sp = ReSendPacketQueue.Instance.Peek();
                //    AgvServerManager.Instance.Send(sp);
                //}
            }
            catch (Exception e)
            {
                Logs.Error("ReSendpacketThread" + e);
            }
        }
    }
}
