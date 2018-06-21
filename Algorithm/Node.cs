using AGV_V1._0.Agv;

namespace Agv.PathPlanning
{
    class Node
    {
        public int x;               //节点的横坐标    
        public int y;               //节点的纵坐标
        public bool node_Type;      //节点可不可达,true表示可达，false表示不可达  
        public int adjoinNodeCount;  //邻接点的个数
        public Direction direction; //搜索到当前节点时的方向
        public int isSearched;     //节点是否被搜索过
        public string tag;
        public string status;


        public int traCongesIntensity;//traffic congestion intensity 节点拥堵程度
        //节点通行难度,数值越大表示越难通行,默认为2，中等通行难度，不可通行用一个非常大的数表示（100）
        public const int UNABLE_PASS = 100;
        public const int MAX_ABLE_PASS = 10;
        public int upDifficulty;
        public int downDifficulty;
        public int leftDifficulty;
        public int rightDifficulty;



    }
}
