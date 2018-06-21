﻿using AGV_V1._0.Event;

namespace AGV_V1._0.Network.Messages
{
    class ReStartMessage:TextMessage
    {
      
        public override void Receive()
        {
            OnMessageEvent("reStart");
            OnTransmitEvent(this, new MessageEventArgs(MessageType.ReStart, this.Message));
        }
        public ReStartMessage()
        {
            this.Type = MessageType.ReStart;
        }

        public override BaseMessage Create(string msg)
        {
            ReStartMessage dis = new ReStartMessage();
            dis.Message = msg;
            return dis;
        }
    }
}
