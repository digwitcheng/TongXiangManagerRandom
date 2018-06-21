using AGV_V1._0.Event;

namespace AGV_V1._0.Network.Messages
{
    class AgvFileMessage:FileMessage
    {
       
        public AgvFileMessage()
        {
            this.Type = MessageType.AgvFile;
        }
        public override BaseMessage Create(string msg)
        {
            AgvFileMessage dis = new AgvFileMessage();
            dis.Message = msg;
            return dis;
        }
    }
}
