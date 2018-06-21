using AGV_V1._0.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGV_V1._0.Agv
{
    class VehicleConfiguration
    {
        public VehicleConfiguration()
        {
            TimerInterval = 100;
            ForwordStep = 5;
            Deviation = 100;
            PathPlanningAlgorithm = new Astar();
        }
        public int TimerInterval { get; set; }
        public int ForwordStep { get; set; }//锁定多少格(包括自己所在的位置)
       // public int StopTime { get; } //等待时长，超过则重新规划路线；
        public int Deviation { get; set; }//坐标相差在DEVIATION以内就看作在一个点,也就是小车坐标精度
        public IAlgorithm PathPlanningAlgorithm { get; set; }
    }
}
