using AGVSocket.Network.EnumType;
using System;

namespace AGVSocket.Network
{
    class CurrentLocation
    {
        private CellPoint curNode;
        private CellPoint desNode;
        private UInt16 speed;
        private MoveDirection moveDir;
        private AgvDriftAngle agvAngle;
        #region Properties
        public CellPoint CurNode { get { return curNode; } }
        public CellPoint DesNode { get { return desNode; } }
        public UInt16 Speed { get { return speed; } }
        public MoveDirection MoveDir { get { return moveDir; } }
        public AgvDriftAngle AgvAngle { get { return agvAngle; } }
       

        #endregion

        public CurrentLocation(byte[] data,ref int offset)
        {
            UInt32 curX = MyBitConverter.ToUInt32(data,ref offset);
            UInt32 curY = MyBitConverter.ToUInt32(data,ref offset);
            this.curNode = new CellPoint(curX, curY);
            UInt32 desX = MyBitConverter.ToUInt32(data, ref offset);
            UInt32 desY = MyBitConverter.ToUInt32(data,ref offset);
            this.desNode = new CellPoint(desX, desY);
            this.speed = MyBitConverter.ToUInt16(data, ref offset);
            this.moveDir = (MoveDirection)data[offset++];
            this.agvAngle =new AgvDriftAngle( MyBitConverter.ToUInt16(data,ref offset));//车头方向采用的是高位在后
        }
    }
}
