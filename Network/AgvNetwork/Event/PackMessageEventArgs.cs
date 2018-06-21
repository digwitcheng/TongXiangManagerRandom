using AGVSocket.Network;
using System;

namespace AGV.Event
{
    public class PackMessageEventArgs:EventArgs
    {
        public string Message { get; set; }
        public PacketType Type { get; set; }
        public PackMessageEventArgs(string msg)
        {
            Message = msg;
        }
        public PackMessageEventArgs(PacketType type, string msg)
        {
            Message = msg;
            Type = type;
        }
    }
}
