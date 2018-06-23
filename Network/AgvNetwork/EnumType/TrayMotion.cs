namespace AGVSocket.Network.EnumType
{
    enum TrayMotion:byte
    {
        /// <summary>
        /// 无动作
        /// </summary>
        None = 0x00,

        /// <summary>
        /// 车头方向，0度（X轴正轴方向）
        /// </summary>
        XPositive  =0x01,

        /// <summary>
        /// 车头方向，90度（Y轴正轴方向）
        /// </summary>
        YPositive = 0x02,

        /// <summary>
        ///车头方向，180度（X轴负轴方向）
        /// </summary>
        XNegative = 0x03,

        /// <summary>
        ///车头方向，270度（Y轴负轴方向）
        /// </summary>
        YNegative = 0x04
    }
}
