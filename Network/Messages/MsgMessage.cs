﻿using AGV_V1._0.Event;

namespace AGV_V1._0.Network.Messages
{
    class MsgMessage : TextMessage
    {
       

        public override void Receive()
        {
            OnMessageEvent("msg");
            OnTransmitEvent(this, new MessageEventArgs(MessageType.Msg, this.Message));
           // Console.WriteLine("MsgMessage:" + this.ShowMessage);
        }
        public MsgMessage()
        {
            this.Type = MessageType.Msg;
        }
        public override BaseMessage Create(string msg)
        {
            MsgMessage dis = new MsgMessage();
            dis.Message = msg;
            return dis;
        }
    }
}
