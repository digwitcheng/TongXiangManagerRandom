using AGVSocket.Network.Packet;

namespace AGV_V1._0.Queue
{
    class SendPacketQueue:BaseQueue<SendBasePacket>
    {
        private static SendPacketQueue instance;
        public static SendPacketQueue Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SendPacketQueue();
                }
                return instance;
            }
        }
        private SendPacketQueue() { }
    }
}
