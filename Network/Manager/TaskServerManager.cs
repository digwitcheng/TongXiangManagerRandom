﻿using AGV_V1._0.Event;
using AGV_V1._0.Network.Messages;
using AGV_V1._0.Util;
using Cowboy.Sockets;
using System;
using System.Text;

namespace AGV_V1._0.Server.APM
{
    class TaskServerManager : ServerManager
    {

        private static TaskServerManager instance;
        public static TaskServerManager Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new TaskServerManager();
                }
                return instance;
            }
        }
        private TaskServerManager() { }
        /// <summary>
        /// 监听本地ip
        /// </summary>
        /// <param name="port">监听的端口号</param>
        public void StartServer(int port)
        {
            var config = new TcpSocketServerConfiguration();
            config.FrameBuilder = new LengthPrefixedFrameBuilder();
            _server = new TcpSocketServer(port, config);
            _server.ClientConnected += server_ClientConnected;
            _server.ClientDisconnected += server_ClientDisconnected;
            _server.ClientDataReceived += server_ClientDataReceived;
            _server.Listen();

        }

        public override void server_ClientConnected(object sender, TcpClientConnectedEventArgs e)
        {
            string str = string.Format("TCP client {0} has connected.", e.Session.RemoteEndPoint);
            Console.WriteLine(str);
            OnMessageEvent(this, new MessageEventArgs(str));

            string pathAgv = ConstDefine.AGV_PATH;
            SendTo(e.Session.SessionKey, MessageType.AgvFile, pathAgv, false);
        }

        public override void server_ClientDisconnected(object sender, TcpClientDisconnectedEventArgs e)
        {
            string str = string.Format("TCP client {0} has disconnected.", e.Session.RemoteEndPoint);
            Console.WriteLine(str);
            OnMessageEvent(this, new MessageEventArgs(str));
        }

        public override void server_ClientDataReceived(object sender, TcpClientDataReceivedEventArgs e)
        {
            MessageType type = (MessageType)e.Data[e.DataOffset];
            var text = Encoding.UTF8.GetString(e.Data, e.DataOffset + 1, e.DataLength - 1);


            BaseMessage bm = BaseMessage.Factory(type, text);
            // bm.ShowMessage += OnMessageEvent; //任务是以消息的形式发送过来的，显示的话会刷屏
            bm.DataMessage += OnDataMessageEvent;
            // bm.ReLoad += ReLoadDel;          
            bm.Receive();
        }

        public void Send(MessageType type, string json)
        {

            try
            {
                //if (false == isSendFile)
                //{
                SendTo("", type, json, true);
                //}
            }
            catch
            {
                //throw;
            }

        }

    }
}
