using AGV_V1._0;
using AGV_V1._0.Agv;
using AGVSocket.Network.MyException;
using System;
using System.Diagnostics;

namespace AGVSocket.Network.Packet
{
    class DoneReplyPacket:ReceiveBasePacket
    {
        private OprationState doneStyle;  //操作完成标识
        public DoneReplyPacket(byte[] data)
            : base("DoneReplyPacket", data) 
        {
            this.doneStyle = (OprationState)data[7];
        }


        public override void Receive()
        {
                Debug.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss.fff") + "]"+this.AgvId + "完成标识:{0},消息是否正确：{1},序列号:{2}", doneStyle, this.IsCheckSumCorrect, this.SerialNum);
           this.ReceiveResponse();

        }

        public override byte NeedLen()
        {
            return 9;
        }
    }
}
