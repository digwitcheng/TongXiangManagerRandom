using System;

namespace AGVSocket.Network.MyException
{
    [Serializable]
    class PacketException:Exception
    {
        public string Message { get; private set; }
        public ExceptionCode Code { get; private set; }
        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="message">出错位置，变量等信息说明</param>
        /// <param name="code">出错类型</param>
        public PacketException(string message,ExceptionCode code)
        {
            Message = message+":"+code;
            Code = code;
        }

    }
}
