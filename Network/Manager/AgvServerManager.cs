using AGVSocket.Network.Packet;
using System;
using AGVSocket.Network.MyException;
using System.Diagnostics;
using System.Collections.Concurrent;
using AGV_V1._0.Server.APM;
using AGV_V1._0.Event;
using AGV_V1._0.NLog;
using Cowboy.Sockets;
using AGV_V1._0.Network.Server.APM;

namespace AGVSocket.Network
{
    class AgvServerManager:ServerManager
    {

       
        private readonly ConcurrentDictionary<ushort, AgvInfo> agvInfo = new ConcurrentDictionary<ushort, AgvInfo>();
        public ConcurrentDictionary<ushort, AgvInfo> GetAgvInfo
        {
            get
            {
                return agvInfo;
            }
        }
        private static AgvServerManager instance;
        public static AgvServerManager Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new AgvServerManager();
                }
                return instance;
            }
        }
        private AgvServerManager() { }
        /// <summary>
        /// 监听本地ip
        /// </summary>
        /// <param name="port">监听的端口号</param>
        public void StartServer(int port)
        {
            var config = new TcpSocketServerConfiguration();
            config.FrameBuilder = new LengthPrefixedOneByteFrameBuilder();
            config.SessionKeyBuilder = new AgvIdSessionKeyBuilder();
            _server = new TcpSocketServer(port, config);
            _server.ClientConnected += server_ClientConnected;
            _server.ClientDisconnected += server_ClientDisconnected;
            _server.ClientDataReceived += server_ClientDataReceived;
            _server.Listen();

        }

        public override void server_ClientConnected(object sender, Cowboy.Sockets.TcpClientConnectedEventArgs e)
        {
            string str = string.Format("小车连接成功，"+e.Session.RemoteEndPoint);
            Console.WriteLine(str);
            OnMessageEvent(this, new MessageEventArgs(str));
        }

        public override void server_ClientDisconnected(object sender, Cowboy.Sockets.TcpClientDisconnectedEventArgs e)
        {
            string str = string.Format("小车断开连接，"+ e.Session.RemoteEndPoint);
            Console.WriteLine(str);
            OnMessageEvent(this, new MessageEventArgs(str));
        }

        public override void server_ClientDataReceived(object sender, Cowboy.Sockets.TcpClientDataReceivedEventArgs e)
        {
           //  var text= Encoding.ASCII.GetChars(e.Data, e.DataOffset, e.DataLength);
           //// var text = Encoding.UTF8.GetString(e.Data, e.DataOffset, e.DataLength);
           //// OnMessageEvent(this, new PackMessageEventArgs(text));
           // Console.WriteLine(text);
            //

            if (e==null||e.DataLength < 8)//
            {
                Debug.WriteLine("data error!,len<8");
                Logs.Error("data error!,len<8");
                return;
            }
            PacketType type = (PacketType)e.Data[e.DataOffset + 4];
            byte[] buffers = new byte[e.DataLength];
            Buffer.BlockCopy(e.Data, e.DataOffset, buffers, 0, e.DataLength);
            // var text = Encoding.UTF8.SetBytes(e.Data, e.DataOffset, e.DataLength);

            
            try
            {
                ReceiveBasePacket rbp = ReceiveBasePacket.Factory(type, buffers);
                //bp.ShowMessage += OnMessageEvent;
                //bp.ErrorMessage += OnErrorEvent;
                //bp.DataMessage += OnDataMessageEvent;
                //bp.ReLoad += ReLoadDel; 
                rbp.Receive();
                
            }
            catch (PacketException ex)
            {
                HanderPacketException(ex);
            }
            catch (Exception ex)
            {
                Logs.Error("未知错误:"+ex);
            }
             
           
        }       

        private void HanderPacketException(PacketException ex)
        {
            Logs.Error("接收错误 "+ex.Code + "-" + ex.Data);
        }
        //public void Send(SendBasePacket pack)
        //{
        //    try
        //    {
        //        pack.Send(_server);
        //    }
        //    catch { }
        //}
        public void SendTo(SendBasePacket pack,int vnum)
        {
            try
            {
               
                //if (vnum == 5)
                //{
                //    vnum = 25;
                //}
                pack.SendTo(_server,vnum);
            }
            catch { }
        }
        public void AddOrUpdate(ushort agvId, AgvInfo info)
        {
            agvInfo.AddOrUpdate(agvId, info, (key, oldValue) => info);
        }
    }
}
