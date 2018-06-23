using AGVSocket.Network;
using AGVSocket.Network.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGV_V1._0.Network.AgvNetwork.Packet
{
    class ClearFalutPacket : SendBasePacket
    {
        public ClearFalutPacket(byte serialNum, ushort agvId)
        {
            this.Header = 0xAA55;
            this.Len = NeedLen();
            this.SerialNum = serialNum;
            this.Type = (byte)PacketType.ClearFault;
            this.AgvId = agvId;
        }
        public override byte[] GetBytes()
        {
            byte[] packData = new byte[NeedLen()];
            int offset = FillBytes(packData);
            packData[NeedLen() - 1] = CaculateCheckSum(packData);
            this.CheckSum = packData[NeedLen() - 1];
            return packData;
        }

        protected override byte NeedLen()
        {
            return 8;
        }
    }
}
