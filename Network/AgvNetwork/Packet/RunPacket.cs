using AGVSocket.Network.EnumType;
using System;

namespace AGVSocket.Network.Packet
{
    class RunPacket:SendBasePacket
    {
        private MoveDirection direction;    //行驶方向 1-正向行驶，2-反向行驶（倒车行
        private ushort speed;            //行驶速度  每秒毫米，如1.5米/秒：1500（0x05DC） 
        private Destination locations;



        public RunPacket(byte serialNum, ushort agvId, MoveDirection direction, ushort speed, Destination location)
        {
            this.Header = 0xAA55;
            this.Len = NeedLen();
            this.SerialNum = serialNum;
            this.Type = (byte)PacketType.Run;
            this.AgvId = agvId;
            this.direction = direction;
            this.speed = speed;
            this.locations = location;
            
        }


        public override  byte[] GetBytes()
        {
            byte[] packData = new byte[Len];
            int offset=FillBytes(packData);

            packData[offset++] = (byte)this.direction;
            Buffer.BlockCopy(MyBitConverter.GetBytes(speed), 0, packData, offset, 2);
            offset+=2;
            Buffer.BlockCopy(locations.GetBytes(), 0, packData, offset, 19);

            packData[NeedLen() - 1] = CaculateCheckSum(packData);
            this.CheckSum = packData[NeedLen() - 1];
            return packData;
        }

        //public  BasePacket Create(byte[] rawMessage)
        //{
        //    if (rawMessage[rawMessage.Length - 1] != GetCheckSum())
        //    {
        //        return new ErrorPacket();
        //    }
        //    RunPacket runPack = pack;
        //    return runPack;
        //}

        //public override BasePacket Create(string msg)
        //{
        //    
        //}

        //public override void Send(string sessionKey, Cowboy.Sockets.TcpSocketServer server)
        //{
        //    
        //}

        //public override void Receive()
        //{
        //    
        //}

        protected override byte NeedLen()
        {
            return 30;
        }
    }
}
