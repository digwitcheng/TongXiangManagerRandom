using AGV_V1._0.Event;

namespace AGV_V1._0.Network.Messages
{
    class MapFileMessage:FileMessage
    {
        public MapFileMessage()
        {
            this.Type = MessageType.MapFile;
        }
        public override BaseMessage Create(string msg)
        {
            MapFileMessage dis = new MapFileMessage();
            dis.Message = msg;
            return dis;
        }
    }
}
