using Agv.PathPlanning;
using AGV_V1._0.Agv;
using System.Collections.Generic;

namespace AGV_V1._0.Algorithm
{
    class SearchUtil
    {

        public const int Sequential = 0;    //顺序遍历
        public const int NoSolution = 2;    //无解决方案
        public const int Infinity = 0xfffffff;
        internal const int MaxLength = 6000;   //用于优先队列（Open表）的数组

        public static MyPoint[] dir = new MyPoint[4]{	
        new MyPoint( 0, 1,Direction.Left),   //,Direction.RightDifficulty // East 0
	 //   new myPoint( 1, 1, ),   // South_East 1
	    new MyPoint( 1, 0,Direction.Left),  //,Direction.DownDifficulty // South 2
	  //  new myPoint(1, -1 ),  // South_West 3
	    new MyPoint( 0, -1,Direction.Left ), //,Direction.LeftDifficulty // West 4
	 //   new myPoint( -1, -1 ), // North_West 5
        new MyPoint( -1, 0,Direction.Left), //,Direction.UpDifficulty  // North 6
	 //   new myPoint( -1, 1)   // North_East 7
        };
        public static void AsatrPush(List<Close> q, Close[,] cls, int x, int y, double g)
        {    //向优先队列（Open表）中添加元素
            Close t;
            int i, mintag;
            cls[x, y].G = g;    //所添加节点的坐标
            cls[x, y].F = cls[x, y].G + cls[x, y].H;

            q.Add(cls[x, y]);
            mintag = 0;
            for (i = 0; i < q.Count - 1; i++)
            {
                if (q[i].F < q[mintag].F)
                {
                    mintag = i;
                }
            }
            t = q[0];
            q[0] = q[mintag];
            q[mintag] = t;    //将评价函数值最小节点置于列表最前面
        }

        public static void DijkstraPush(List<Close> q, Close[,] cls, int x, int y, double g)
        {    //向优先队列（Open表）中添加元素
            Close t;
            int i, mintag;
            cls[x, y].G = g;    //所添加节点的坐标
            cls[x, y].F = cls[x, y].G;// + cls[x, y].H;

            q.Add(cls[x, y]);
            mintag = 0;
            for (i = 0; i < q.Count - 1; i++)
            {
                if (q[i].F < q[mintag].F)
                {
                    mintag = i;
                }
            }
            t = q[0];
            q[0] = q[mintag];
            q[mintag] = t;    //将评价函数值最小节点置于列表最前面
        }
        public static Direction getDirection(Close from, Close curPoint)
        {

            if (from.Node.x - curPoint.Node.x == 1)
            {
                return Direction.Up;// 3 North;
            }
            if (from.Node.x - curPoint.Node.x == -1)
            {
                return Direction.Down;// 1;//South;
            }
            if (from.Node.y - curPoint.Node.y == 1)
            {
                return Direction.Left;// 2;//West;
            }
            if (from.Node.y - curPoint.Node.y == -1)
            {
                return Direction.Right;// 0;// East;
            }
            return from.Node.direction;
        }
        public static Close shift(List<Close> q)
        {
            Close node = q[0];
            q.RemoveAt(0);
            return node;
        }
       
    }
}
