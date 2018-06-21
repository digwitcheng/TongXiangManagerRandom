using Agv.PathPlanning;
using AGV_V1._0.Agv;
using System;
using System.Collections.Generic;

namespace AGV_V1._0.Algorithm
{
    class Dijkstra:IAlgorithm
    {
        const int SWERVE_COST = 3;
        private int Height = 10;
        private int Width = 10;
        private int beginX;
        private int beginY;
        private int endX;
        private int endY;
        // 地图Close表初始化配置
        void initClose(Close[,] cls, Node[,] graph)
        {
            int i, j;
            for (i = 0; i < Height; i++)
            {
                for (j = 0; j < Width; j++)
                {
                    cls[i, j] = new Close { };
                    cls[i, j].Node = graph[i, j];               // Close表所指节点
                    cls[i, j].Node.isSearched = -1;// !(graph[i, j].node_Type);  // 是否被访问
                    cls[i, j].From = null;                    // 所来节点
                    cls[i, j].G = 0;  //需要根据前面的点计算
                    cls[i, j].H = Math.Abs(endX - i) + Math.Abs(endY - j);    // 评价函数值
                    cls[i, j].F = 0;  //cls[i, j].G + cls[i, j].H;
                }
            }
            //cls[endX, endY].G = AstarUtil.Infinity;     //移步花费代价值
            //cls[beginX, beginY].F = cls[beginX, beginY].H;            //起始点评价初始值
        }
        public List<MyPoint> Search(Node[,] graph, int beginX, int beginY, int endX, int endY, Direction beginDir)
        {
            List<MyPoint> route = new List<MyPoint>();
            Height = graph.GetLength(0);
            Width = graph.GetLength(1);
            this.beginX = beginX;
            this.beginY = beginY;
            this.endX = endX;
            this.endY = endY;
            Close[,] close = new Close[Height, Width];
            initClose(close, graph);
            close[beginX, beginY].Node.isSearched = 0;
            // A*算法遍历
            //int times = 0; 
            int i, curX, curY, nextX, nextY;
            double surG;
            Close curPoint = new Close();

            List<Close> open = new List<Close>();
            SearchUtil.DijkstraPush(open, close, beginX, beginY, 0);
            int times = 0;
            bool isFirstDirection = true;
            int searchLevel = 0;
            while (open.Count > 0)
            {
                times++;
                curPoint = SearchUtil.shift(open);
                curX = curPoint.Node.x;
                curY = curPoint.Node.y;

                if (curPoint.From == null)
                {
                    curPoint.Node.direction = beginDir;
                }
                else
                {
                    isFirstDirection = false;
                    curPoint.Node.direction = SearchUtil.getDirection(curPoint.From, curPoint);//0525
                }
                for (i = 0; i < 4; i++)
                {
                    if ((curPoint.Node.adjoinNodeCount & (1 << i)) == 0)
                    {
                        continue;
                    }
                    nextX = curX + (int)SearchUtil.dir[i].X;
                    nextY = curY + (int)SearchUtil.dir[i].Y;
                    if (close[nextX, nextY].Node.isSearched < 0)
                    {
                        close[nextX, nextY].Node.isSearched = searchLevel++;
                        close[nextX, nextY].From = curPoint;
                        Direction tempDir = new Direction();
                        int tempPassDifficulty = 2;
                        switch (i)
                        {
                            case 0:
                                tempDir = Direction.Right;
                                tempPassDifficulty = close[curX, curY].Node.rightDifficulty;
                                break;
                            case 1:
                                tempDir = Direction.Down;
                                tempPassDifficulty = close[curX, curY].Node.downDifficulty;
                                break;
                            case 2:
                                tempDir = Direction.Left;
                                tempPassDifficulty = close[curX, curY].Node.leftDifficulty;
                                break;
                            case 3:
                                tempDir = Direction.Up;
                                tempPassDifficulty = close[curX, curY].Node.upDifficulty;
                                break;
                        }
                        int directionCost = (tempDir == curPoint.Node.direction) ? 0 : 2;
                        if (directionCost == 2 && isFirstDirection == true)
                        {
                            // directionCost--;
                        }
                        int tempTraConges = close[curX, curY].Node.traCongesIntensity;

                        surG = curPoint.G + (float)(Math.Abs(curX - nextX) + Math.Abs(curY - nextY)) + SWERVE_COST * (directionCost + tempTraConges) + tempPassDifficulty;
                        SearchUtil.DijkstraPush(open, close, nextX, nextY, surG);
                    }
                }
                if (curPoint.H == 0)
                {
                    System.Console.WriteLine("astar times:" + times);
                    GetShortestPath(route, close);
                    return route;
                }
            }
            return route; //无结果
        }
        /// <summary>
        /// // 获取最短路径
        /// </summary>
        /// <returns></returns>
        void GetShortestPath(List<MyPoint> route, Close[,] close)
        {
            Close p = (close[endX, endY]);
            while (p != null)    //转置路径
            {
                route.Add(new MyPoint(p.Node.x, p.Node.y,p.Node.direction));
                p = p.From;
            }
            route.Reverse();
            
        }

    }
}
