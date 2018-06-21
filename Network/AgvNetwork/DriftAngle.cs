namespace AGVSocket.Network
{
    /// <summary>
    /// 车头偏转角度0~360
    /// </summary>
    class AgvDriftAngle
    {
        private ushort angle;
        public ushort Angle { get { return angle; } }

        public AgvDriftAngle(ushort angle)
        {
            this.angle = angle;
        }
    }
}
