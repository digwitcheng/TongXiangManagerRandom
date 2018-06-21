//#define moni

using AGV_V1._0.Agv;
using AGV_V1._0.Algorithm;
using AGV_V1._0.Event;
using AGV_V1._0.Network.ThreadCode;
using AGV_V1._0.NLog;
using AGV_V1._0.Queue;
using AGV_V1._0.Util;
using AGVSocket.Network;
using AGVSocket.Network.EnumType;
using AGVSocket.Network.Packet;
using Agv.PathPlanning;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using AGV_V1._0.Network.AgvNetwork.Packet;
using AGVSocket.Network.MyException;

namespace AGV_V1._0
{
    class VehicleManager : BaseThread
    {
        //新建两个全局对象  小车
        private static Vehicle[] vehicles;
        List<Vehicle> vFinished = new List<Vehicle>();
        private bool vehicleInited = false;
        private double moveCount = 0;//统计移动的格数，当前地图一格1.5米
        public const int REINIT_COUNT = 20;
        private const int WAIT_TIME = 8;//  等待超时后还没有翻盘完成的消息就重发翻盘报文

        private static Random rand = new Random(1);//5,/4/4 //((int)DateTime.Now.Ticks);//随机数，随机产生坐标


        private static VehicleManager instance;
        public static VehicleManager Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new VehicleManager();
                }
                return instance;
            }
        }
        private VehicleManager()
        {

        }
        protected override string ThreadName()
        {
            return "VehicleManager";
        }
        protected override void Run()
        {
            Thread.Sleep(ConstDefine.STEP_TIME);
            if (vehicles == null)
            {
                return;
            }
            for (int vnum =3; vnum <4; vnum++)
            {
                if (vehicles[vnum].agvInfo == null)
                {
                    continue;
                }
                if (vehicles[vnum].CurState == State.cannotToDestination && vehicles[vnum].Arrive == false)
                {
                    vehicles[vnum].Arrive = true;
                    vehicles[vnum].CurState = State.Free;
                    vFinished.Add(vehicles[vnum]);
                    ClearAllCrossedNode(vnum);
                    
                    string str = string.Format("小车" + vnum + ":{0},{1}->{2},{3}没有搜索到路径，", vehicles[vnum].BeginX, vehicles[vnum].BeginY, vehicles[vnum].EndX, vehicles[vnum].EndY);
                    OnShowMessage(this, new MessageEventArgs(str));
                    continue;
                }
                if (vehicles[vnum].Finished == true)
                {
                    continue;
                }
                if(vehicles[vnum].IsAgvReceived == false)
                {
                    continue;
                }

                if (vehicles[vnum].Arrive == true && vehicles[vnum].CurState == State.carried)
                {
                    if (vehicles[vnum].EqualWithRealLocation(vehicles[vnum].BeginX, vehicles[vnum].BeginY))
                    {
                           if (vehicles[vnum].agvInfo.OrderExec ==OrderExecState.Free
                            )
                        {
                            TrayPacket tp = new TrayPacket((byte)(vehicles[vnum].Serinum), (ushort)vnum, TrayMotion.TopLeft); 
                            SendPack(vnum,tp);
                            vehicles[vnum].CurState = State.unloading;                            
                        }
                        continue;
                    } 
                }               
                //if (vehicles[vnum].Arrive == true && vehicles[vnum].CurState == State.unloading)
                //{
                //    if (vehicles[vnum].WaitEndTime < DateTime.Now)//超过等待时间还不能走，则重新发送一下倒货
                //    {
                //        if (vehicles[vnum].EqualWithRealLocation(vehicles[vnum].BeginX, vehicles[vnum].BeginY))
                //        {
                //            if (vehicles[vnum].agvInfo.AgvMotion == AgvMotionState.StopedNode)
                //            {
                //                TrayPacket tp = new TrayPacket((byte)(vehicles[vnum].Serinum++), (ushort)vnum, TrayMotion.TopLeft);
                //                SendPack(vnum, tp);
                //                vehicles[vnum].CurState = State.unloading;
                //            }
                //            continue;
                //        }
                //    }
                //}
                
                if (vehicles[vnum].Arrive == true && vehicles[vnum].CurState == State.unloading)
                {
                    if (vehicles[vnum].agvInfo.OrderExec == OrderExecState.Free)
                    {
                        vFinished.Add(vehicles[vnum]);
                        ClearAllCrossedNode(vnum);
                        vehicles[vnum].Finished = true;
                        vehicles[vnum].CurState = State.Free;
                    }
                     continue;
                }
                Random r = new Random(DateTime.Now.Millisecond);
                if (vehicles[vnum].StopTime <r.Next(vnum%5) )
                {
                    vehicles[vnum].CurState = State.cannotToDestination;
                    continue;

                    //if (vehicles[vnum].CurNodeTypy() != MapNodeType.queuingArea && GetDirCount(vehicles[vnum].BeginX, vehicles[vnum].BeginY) > 1)
                    //{
                    //    if (vehicles[vnum].Stoped > -1 && vehicles[vnum].Stoped < vehicles.Length)
                    //    {
                    //        vehicles[vehicles[vnum].Stoped].StopTime = ConstDefine.STOP_TIME;
                    //    }
                    //    //重新搜索路径
                    //    SearchRoute(vnum, true);
                    //}
                }
                else
                {
                   MoveType moveState = vehicles[vnum].Move(ElecMap.Instance);
                    if (moveState==MoveType.move)
                        {
                           uint x = Convert.ToUInt32(vehicles[vnum].BeginX);
                            uint y = Convert.ToUInt32(vehicles[vnum].BeginY);
                            uint endX = Convert.ToUInt32(vehicles[vnum].EndX);
                            uint endY = Convert.ToUInt32(vehicles[vnum].EndY);
                            uint cellUnit = (int)ConstDefine.CELL_UNIT;
                            RunPacket rp = new RunPacket((byte)(vehicles[vnum].Serinum), (ushort)vnum, MoveDirection.Forward, 1500, new Destination(new CellPoint(x * cellUnit, y * cellUnit), new CellPoint(endX * cellUnit, endY * cellUnit), new AgvDriftAngle(0), TrayMotion.None));
                        SendPack(vnum, rp);                        

                        ElecMap.Instance.mapnode[vehicles[vnum].BeginX, vehicles[vnum].BeginY].TraCongesIntensity = 100;//实际场地没用，故用来表示已经发送过的节点的标志，便于在界面上绘制

                        moveCount++;
                        OnShowMessage(string.Format("{0:N} 公里", (moveCount * 1.5) / 1000.0));
                    }
                    if (moveState == MoveType.clearFault)
                    {
                        ClearFalut cf = new ClearFalut(vehicles[vnum].Serinum, (ushort)vnum);
                        SendPack(vnum, cf);
                    }


                }




            }
            if (vFinished != null)
            {
                for (int i = 0; i < vFinished.Count; i++)
                {
                    FinishedQueue.Instance.Enqueue(vFinished[i]);
                }
                vFinished.Clear();
            }

        }

        private void SendPack(int vnum,SendBasePacket sendPacket)
        {
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------");
            Console.Write("[" + DateTime.Now.ToString("HH:mm:ss.fff") + "]"+"<-发送报文类型：" + (PacketType)sendPacket.Type);
            PrintLocation(vnum);
            CheckAlarmState(vnum);
            AgvServerManager.Instance.SendTo(sendPacket, vnum);
            

            vehicles[vnum].LastSendPacket = sendPacket;
            vehicles[vnum].IsAgvReceived = false;
            vehicles[vnum].WaitEndTime = DateTime.Now.AddSeconds(WAIT_TIME);



        }
        void PrintLocation(int vnum)
        {
            uint x = Convert.ToUInt32(vehicles[vnum].BeginX);
            uint y = Convert.ToUInt32(vehicles[vnum].BeginY);
            uint endX = Convert.ToUInt32(vehicles[vnum].EndX);
            uint endY = Convert.ToUInt32(vehicles[vnum].EndY);
            Console.WriteLine(",小车"+vnum + "：(" + x + "," + y + ")终点：(" + endX + "," + endY + "),实际位置:(" + vehicles[vnum].agvInfo.CurLocation.CurNode.X / 1000.0 + "," + vehicles[vnum].agvInfo.CurLocation.CurNode.Y / 1000.0 + ") 序列号：" + vehicles[vnum].Serinum);
        }

        void ClearAllCrossedNode(int vnum)
        {
            vehicles[vnum].LockNode.Clear();
            //for (int i = 0; i < vehicles[vnum].Route.Count - 1; i++)
            //{
            //    vehicles[vnum].SetNodeCanUsed(vehicles[vnum].Route[i].X, vehicles[vnum].Route[i].Y);
            //}
            for(int i = 0; i < ElecMap.Instance.HeightNum; i++)
            {
                for(int j = 0; j < ElecMap.Instance.WidthNum; j++)
                {
                    if (i == vehicles[vnum].GetRealX() && j == vehicles[vnum].GetRealY())
                    {
                        continue;
                    }
                    vehicles[vnum].SetNodeCanUsed(i, j);
                }
            }
           
        }
        void SendSwerveCommand(int vnum,int angle)
        {
            SwervePacket sp = new SwervePacket((byte)(vehicles[vnum].Serinum++), (ushort)vnum,new AgvDriftAngle((ushort)angle));
            AgvServerManager.Instance.SendTo(sp, vnum);
            Console.WriteLine("send Swerver...");
        }
        void CheckAlarmState(int vnum)
        {
            if (vehicles[vnum].agvInfo.Alarm == AlarmState.ScanNone || vehicles[vnum].agvInfo.Alarm == AlarmState.CommunicationFault)
            {
                Logs.Error("通信故障或没扫到码"+vehicles[vnum].agvInfo.Alarm.ToString());
            }
        }
       
        
       
        int GetDirCount(int row, int col)
        {
            int dir = 0;
            if (ElecMap.Instance.mapnode[row, col].RightDifficulty < MapNode.MAX_ABLE_PASS)
            {
                dir++;
            }
            if (ElecMap.Instance.mapnode[row, col].LeftDifficulty < MapNode.MAX_ABLE_PASS)
            {
                dir++;
            }
            if (ElecMap.Instance.mapnode[row, col].DownDifficulty < MapNode.MAX_ABLE_PASS)
            {
                dir++;
            }
            if (ElecMap.Instance.mapnode[row, col].UpDifficulty < MapNode.MAX_ABLE_PASS)
            {
                dir++;
            }
            return dir;
        }
        /// <summary>
        /// 初始化小车
        /// </summary>
        public void InitialVehicle()
        {
            vehicleInited = false;
            //初始化小车位置

            if (null == FileUtil.sendData || FileUtil.sendData.Length < 1)
            {
                throw new ArgumentNullException();
            }
            int vehicleCount = FileUtil.sendData.Length;
            vehicles = new Vehicle[vehicleCount];
            for (int i = 0; i < vehicleCount; i++)
            {
                VehicleConfiguration config = new VehicleConfiguration();
                vehicles[i] = new Vehicle(FileUtil.sendData[i].BeginX, FileUtil.sendData[i].BeginY, i, false, Direction.Right,config);
                //MyPoint endPoint = RouteUtil.RandPoint(ElecMap.Instance);
                //MyPoint mp = SqlManager.Instance.GetVehicleCurLocationWithId(i);
                //if (mp != null)
                //{
                //    vehicles[i].BeginX = mp.X;
                //    vehicles[i].BeginY = mp.Y;
                //}
                int R = rand.Next(20, 225);
                int G = rand.Next(20, 225);
                int B = rand.Next(20, 225);
                vehicles[i].pathColor = Color.FromArgb(80, R, G, B);
                vehicles[i].showColor = Color.FromArgb(255, R, G, B);
            }

            vehicleInited = true;
            ////把小车所在的节点设为占用状态
          //  RouteUtil.VehicleOcuppyNode(ElecMap.Instance, vehicles);

        }
        public void ReInitWithiRealAgv()
        {
            
            bool res = false;
            int count=1;
            while (res == false && count < REINIT_COUNT)
            {
                Thread.Sleep(count * 50);
                for (int i = 0; i < vehicles.Length; i++)
                {
                    if (vehicles[i].agvInfo != null)
                    {

                        vehicles[i].BeginX = vehicles[i].GetRealX();//(int)Math.Round(vehicles[i].agvInfo.CurLocation.CurNode.X/1000.0);
                        vehicles[i].BeginY = vehicles[i].GetRealY();// (int)Math.Round(vehicles[i].agvInfo.CurLocation.CurNode.Y/1000.0);
                        res = true;
                        Console.WriteLine("小车编号" + i + "初始化完成,起点("+vehicles[i].BeginX+","+vehicles[i].BeginY+")");
                    }
                }
                count++;
            }
            ////把小车所在的节点设为占用状态
           // RouteUtil.VehicleOcuppyNode(ElecMap.Instance, vehicles);
            if (count >= REINIT_COUNT)
            {
                MessageBox.Show("没有小车连接，请检查ip设置是否有问题");
            }
        }
        public void AddOrUpdate(ushort agvId, AgvInfo info)
        {
            if (vehicleInited == false)
            {
                return;
            }
            //if (agvId >= vehicles.Length)
            //{
            //    Logs.Error("程序预设的agv数量少了");
            //    return;
            //}
            if (info == null)
            {
                return;
            }
           //
            vehicles[(int)agvId].agvInfo = info;
           // Console.WriteLine(info.CurLocation.AgvAngle.Angle);
        }
        public void RandomMove(int Id)
        {
            // MyPoint mpEnd= new MyPoint(0, 6, Direction.Right);
            MyPoint mpEnd;
            if (vehicles[Id].BeginX == 0) {
               mpEnd = new MyPoint(10, 6, Direction.Right);
            }
            else
            {
                mpEnd = new MyPoint(0, 6, Direction.Right);
            }
            // MyPoint mpEnd =new MyPoint(7,3,Direction.Right);// RouteUtil.RandRealPoint(ElecMap.Instance);
            //while (mpEnd.X == vehicles[Id].BeginX && mpEnd.Y == vehicles[Id].BeginY)
            //{
            //    mpEnd = RouteUtil.RandRealPoint(ElecMap.Instance);
            //}
            SendData sd = new SendData(Id, vehicles[Id].BeginX, vehicles[Id].BeginY, mpEnd.X, mpEnd.Y);
            sd.Arrive = false;
            sd.EndLoc = "rest";
            sd.State = State.carried;

            SearchRouteQueue.Instance.Enqueue(new SearchData(sd, false));


        }
        void SearchRoute(int num, bool isResarch)
        {
            SendData td = new SendData();
            td.Num = num;
            td.BeginX = vehicles[num].BeginX;
            td.BeginY = vehicles[num].BeginY;
            td.EndX = vehicles[num].EndX;
            td.EndY = vehicles[num].EndY;
            td.Arrive = false;
            td.EndLoc = vehicles[num].EndLoc;
            td.StartLoc = vehicles[num].StartLoc;
            td.State = vehicles[num].CurState;


            //if (!ElecMap.Instance.IsSpecialArea(td.BeginX, td.BeginY) && ElecMap.Instance.IsScanner(td.EndX, td.EndY))
            //{
            //    MessageBox.Show("起点：" + td.BeginX + "," + td.BeginY + "" + "终点：" + td.EndX + "," + td.EndY);
            //}
            SearchRouteQueue.Instance.Enqueue(new SearchData(td, isResarch));

            //Task.Factory.StartNew(() => vehicle[num].SearchRoute(Elc), TaskCreationOptions.LongRunning);
        }

        private static readonly object vehicleLock = new object();
        public Vehicle[] GetVehicles()
        {
            //Vehicle[] v = null;
            //lock (vehicleLock)
            //{
            //    if (vehicles != null)
            //    {
            //        v = new Vehicle[ConstDefine.g_VehicleCount];
            //        for (int i = 0; i < vehicles.Length; i++)
            //        {
            //            v[i] = vehicles[i].CloneDeep();
            //        }
            //    }
            //}
            //return v;
            return vehicles;
        }


    }

}
