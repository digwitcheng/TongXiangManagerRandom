using AGVSocket.Network.Packet;

namespace AGV_V1._0.Queue
{
    class ReSendPacketQueue:BaseQueue<SendBasePacket>
    {
        private static ReSendPacketQueue instance;
        public static ReSendPacketQueue Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ReSendPacketQueue();
                }
                return instance;
            }
        }
        private ReSendPacketQueue() { }
    }
}
