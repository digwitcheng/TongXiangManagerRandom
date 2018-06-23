using AGVSocket.Network;
using AGVSocket.Network.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGV_V1._0.Network.AgvNetwork.Packet
{
    class FinishChargePacket : SendBasePacket
    {
        private CellPoint desPoint;         //目标位置
        public FinishChargePacket(byte serialNum, ushort agvId,CellPoint desPoint)
        {
            this.Header = 0xAA55;
            this.Len = NeedLen();
            this.SerialNum = serialNum;
            this.Type = (byte)PacketType.FinishCharge;
            this.AgvId = agvId;
            this.desPoint = desPoint;
        }
        public override byte[] GetBytes()
        {
            byte[] packData = new byte[Len];
            int offset = FillBytes(packData);
            Buffer.BlockCopy(MyBitConverter.GetBytes(desPoint.X), 0, packData, offset, 4);
            offset += 4;
            Buffer.BlockCopy(MyBitConverter.GetBytes(desPoint.Y), 0, packData, offset, 4);
            offset += 4;
            packData[NeedLen() - 1] = CaculateCheckSum(packData);
            this.CheckSum = packData[NeedLen() - 1];
            return packData;
        }

        protected override byte NeedLen()
        {
            return 16;
        }
    }
}
