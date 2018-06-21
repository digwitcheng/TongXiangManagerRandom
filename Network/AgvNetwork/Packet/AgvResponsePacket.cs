using AGV_V1._0;
using AGVSocket.Network.EnumType;
using System;
using System.Diagnostics;

namespace AGVSocket.Network.Packet
{
    class AgvResponsePacket:ReceiveBasePacket
    {
        private PacketType respType; //需要应答报文类型
        private ResponseState respState;//需要应答报文状态

        public AgvResponsePacket(byte[] data)
            : base("AgvResponsePacket", data)
        {
                this.respType = (PacketType)data[7];
                this.respState = (ResponseState)data[8];
            
        }
      

        public override void Receive()
        {
            Debug.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss.fff") + "]"+"->小车{0}应答报文，应答类型{1},是否正确收到：{2},序列号：{3}", this.AgvId,this.respType, this.respState,this.SerialNum);
            Vehicle v = VehicleManager.Instance.GetVehicles()[this.AgvId];
            if (this.respState == ResponseState.Correct)
            {
                v.IsAgvReceived = true;
                v.Serinum++;
            }
            else
            {
                Console.WriteLine("没有正确收到!");
                if (SerialNum == v.Serinum)
                {
                    AgvServerManager.Instance.SendTo(v.LastSendPacket, this.AgvId);
                    Console.WriteLine(AgvId+"resend");
                }
            }
        }

        public override byte NeedLen()
        {
            return 10;
        }
    }
}
