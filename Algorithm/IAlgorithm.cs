using Agv.PathPlanning;
using AGV_V1._0.Agv;
using System.Collections.Generic;

namespace AGV_V1._0.Algorithm
{
    interface IAlgorithm
    {
        List<MyPoint> Search(Node[,] graph, int beginX, int beginY,int endX,int endY, Direction beginDir);
    }
}
