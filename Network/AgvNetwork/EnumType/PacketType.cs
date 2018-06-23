namespace AGVSocket.Network
{
    public enum PacketType : byte 
    {

        //发送的报文类型
        Run = 0xA4,             //运行控制报文 0xA4
        Swerve = 0xA5,          //转向命令报文 0xA5
        Tray=0xA3,              //托盘控制报文 0xA3
        SysResponse=0xA9,       //发给上位机的应答报文 0xA9
        ClearFault=0xA6,        //清错指令
        Charge= 0XA7,           //充电
        FinishCharge = 0XA8,    //退出充电


        //收到的报文类型
        DoneReply =0xB1,         //收到的操作完成的回复报文 0xB1
        AgvInfo=0xB2,           //小车详情报文 0xB2
        AgvResponse=0xB9        //小车返回的应答报文 0xB9

    }
}
