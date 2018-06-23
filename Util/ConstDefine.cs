namespace AGV_V1._0.Util
{
    class ConstDefine
    {


        //public const byte CHECKSUM = 0x7f;

        public const int UPDATA_SQL_TIME = 50;

        public const int minX =0;
        public const int maxX =10;
        public const int minY =0;
        public const int maxY = 11;
        public const float CELL_UNIT = 1000;//格和毫格的转换单位

        public const int REMOTE_PORT = 8081;//远程过程调用端口
        public const string REMOTE_NAME = "RouteSearch";//远程过程调用名称
        public const int VEHICLE_SUM = 10;

        public const int STOP_TIME=25;//等待时长，超过则重新规划路线；
        public const int CLEAR_FAULT_COUNT = 3;


        public static int g_WidthNum = 100;        //地图格子的个数，默认150*150
        public static int g_HeightNum = 100;       //
        public static int g_NodeLength = 12;       //默认边长
        public static int g_VehicleCount = 10;     //小车数量
        
        

        
        public const int STEP_TIME = 200;//小车每走一格的间隔
        public const int GUI_TIME = 200;//发送给界面的间隔
        public const int CHECK_CONGESTION = 3000;//检测拥堵情况
        public const int SEARCH_TIME = 3;//搜索路径的间隔
        public const int TASK_TIME = 5;  //处理任务的间隔
        //public const int RESEARCH_COUNT = 10;
        public const int UNLOADING_TIME = 2000;//在投放口停留的时间
        public const int RESEND_TIME = 100;






        public const string IP_ADRESS = "127.0.0.1";
        public const string GUI_PORT_ADRESS = "5555";
        public const string TASK_PORT_ADRESS = "5556";
        public const int AGV_PORT_ADRESS = 54321;
        public const string AGV_PATH = "..\\..\\Agv\\AGV.xml";
        public const string MAP_PATH = "..\\..\\Map\\ElcMap.xml";
        public const string CONFIG_PATH = "..\\..\\NLog\\NLog.config";
    }
   

   
}
