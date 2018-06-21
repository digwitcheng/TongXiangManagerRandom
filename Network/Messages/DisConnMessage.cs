using AGV_V1._0.Event;
using System;

namespace AGV_V1._0.Network.Messages
{
    class DisConnMessage:TextMessage
    {

        public override void Receive()
        {
            Console.WriteLine("disconn:" +this.Message);
        }
        public DisConnMessage()
        {
            this.Type = MessageType.DisConnect;
        }
        public override BaseMessage Create(string msg)
        {
            DisConnMessage dis = new DisConnMessage();
            dis.Message = msg;
            return dis;
        }
    }
}
