using System;

namespace AGV_V1._0.Event
{
    public class MessageEventArgs:EventArgs
    {
        public string Message { get; set; }
        public MessageType Type { get; set; }
        public MessageEventArgs(string msg)
        {
            Message = msg;
        }
        public MessageEventArgs(MessageType type, string msg)
        {
            Message = msg;
            Type = type;
        }
    }
}
