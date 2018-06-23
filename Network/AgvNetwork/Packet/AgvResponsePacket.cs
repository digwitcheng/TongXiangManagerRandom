using AGV_V1._0;
using AGV_V1._0.NLog;
using AGV_V1._0.Queue;
using AGVSocket.Network.EnumType;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

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

        private static object seriLock = new object();
        public override void Receive()
        {
            Logs.Info(string.Format("->小车{0}应答报文，应答类型{1},是否正确收到：{2},序列号：{3}", this.AgvId,this.respType, this.respState,this.SerialNum));

            //lock(seriLock)
            //{
            Vehicle v = VehicleManager.Instance.GetVehicles()[this.AgvId];
            if (SerialNum == v.Serinum)
            {
                if (this.respState == ResponseState.Correct)
                {
                    v.IsAgvReceived = true;
                    v.Serinum++;
                }
                else
                {
                    Console.WriteLine("没有正确收到!");

                    ReSendPacketQueue.Instance.Enqueue(v);
                    //AgvServerManager.Instance.SendTo(v.LastSendPacket, this.AgvId);
                    //Console.WriteLine(AgvId + "resend");

                }
            }
            else
            {
                Logs.Warn(AgvId + "号小车回复收到，序列号错误！收到序列号：" + SerialNum + "小车序列号" + v.Serinum+ ",应答类型:" + respType);
            }
           // }
        }

        public override byte NeedLen()
        {
            return 10;
        }
    }
}
