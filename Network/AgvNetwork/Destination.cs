﻿using AGVSocket.Network.EnumType;
using System;

namespace AGVSocket.Network
{
    class Destination
    {
        private CellPoint midPoint;         //中间位置
        private CellPoint desPoint;         //目标位置
        private AgvDriftAngle desAngle;    //目标方向(坐标x轴正方向的夹角)
        private TrayMotion desMotion;   //目标动作

       

        public Destination(CellPoint midPoint, CellPoint desPoint, AgvDriftAngle desAngle, TrayMotion motion)
        {
            this.midPoint = midPoint;
            this.desPoint = desPoint;
            this.desAngle = desAngle;
            this.desMotion = motion;

        }
        public byte[] GetBytes()
        {
            byte[] packData = new byte[19];
            int offset = 0;
            Buffer.BlockCopy(MyBitConverter.GetBytes(midPoint.X), 0, packData, offset, 4);
            offset += 4;     
            Buffer.BlockCopy(MyBitConverter.GetBytes(midPoint.Y), 0, packData, offset, 4);
            offset += 4;     
            Buffer.BlockCopy(MyBitConverter.GetBytes(desPoint.X), 0, packData, offset, 4);
            offset += 4;     
            Buffer.BlockCopy(MyBitConverter.GetBytes(desPoint.Y), 0, packData, offset, 4);
            offset += 4;     
            Buffer.BlockCopy(MyBitConverter.GetBytes(desAngle.Angle), 0, packData, offset, 2);
            offset += 2;
            packData[offset++] = (byte)desMotion;
            return packData;
        }
       
    }
}
