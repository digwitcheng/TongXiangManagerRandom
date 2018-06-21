﻿using System;

namespace AGVSocket.Network.Packet
{
    class SwervePacket:SendBasePacket
    {
        private AgvDriftAngle desDir;//目标方向        
        protected override byte NeedLen()
        {
            return 10;
        }
        public SwervePacket(byte serialNum, ushort agvId, AgvDriftAngle direction)
        {
            this.Header = 0xAA55;
            this.Len = NeedLen();
            this.SerialNum = serialNum;
            this.Type = (byte)PacketType.Swerve;
            this.AgvId = agvId;
            this.desDir = direction;
        }
        public override byte[] GetBytes()
        {
            byte[] packData = new byte[NeedLen()];
             int offset=  FillBytes(packData);

            Buffer.BlockCopy(MyBitConverter.GetBytes(desDir.Angle), 0, packData, offset, 2);

            packData[NeedLen() - 1] = CaculateCheckSum(packData);
            this.CheckSum = packData[NeedLen() - 1];

            return packData;
        }

    }
}
