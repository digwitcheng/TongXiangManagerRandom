using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agv.PathPlanning;
using AGV_V1._0.Agv;

namespace AGV_V1._0.Algorithm
{
    

    class Dstar : IAlgorithm
    {
        Queue<Close> OpenList = new Queue<Close>();
        int correnti, correntj;
        private int hp = 10;
        private int wp = 10;
        private int beginX;
        private int beginY;
        private int endX;
        private int endY;

        //INSERT THE ELEMENT S INTO OPENLIST
         void insert(Queue<Close> OpenList, Close s)
        {
            s.Node.tag = "OPEN";
            OpenList.Enqueue(s);
        }//end of insert
        //GIVING VALUES TO SI,SJ (THE PROBEBLY VALUE OF NEIGHBOURS OF STATE IN [IOLD,JOLD] POSITION)
         void vs(int[] si, int[] sj, int iold, int jold)
        {
            //si[0] = iold - 1; sj[0] = jold + 1;

            si[1] = iold; sj[1] = jold + 1;

            //si[2] = iold + 1; sj[2] = jold + 1;

            si[3] = iold - 1; sj[3] = jold;

            si[4] = iold + 1; sj[4] = jold;

            //si[5] = iold - 1; sj[5] = jold - 1;

            si[6] = iold; sj[6] = jold - 1;

            //si[7] = iold + 1; sj[7] = jold - 1;
        }//end of vs
         //THE PROCESS STEPS IN "CROSS" MOVING TO NIEGHBOURS
         void crossmove(int si, int sj, Close s, Close old, Queue<Close> OpenList)
        {
            if (s.Node.tag == "NEW")
            {
                if (s.Node.node_Type == false)
                    s.H = 10000;
                else
                    s.H = old.H + 1.4;
                s.G = s.H;
                insert(OpenList, s);
                s.From = old;
            }

        }//end of cossmove


        //THE PROCESS STEPS IN "HORIZONTALY" OR "VERTICALLY" MOVING TO NIEGHBOURS
         void linemove(int si, int sj, Close s, Close old, Queue<Close> OpenList)
        {

            if (s.Node.tag == "NEW")
            {
                if (s.Node.node_Type == false)
                    s.H = 10000;
                else s.H = old.H + 1;
                s.G = s.H;
                insert(OpenList, s);
                s.From = old;
            }

        }//end of linemove

         void sortqueue(Queue<Close> OpenList, Close[,] state)
        {
            int ind = OpenList.Count;

            double[] kk = new double[ind];
            int[] ii = new int[ind];
            int[] jj = new int[ind];

            Close d = new Close();

            for (int i = 0; i < ind; i++)
            {
                d = (Close)OpenList.Dequeue();
                kk[i] = d.G;
                ii[i] = d.Node.x;
                jj[i] = d.Node.y;
            }

            for (int i = 0; i < ind; i++)
                for (int j = 0; j < ind; j++)
                {
                    if (kk[i] < kk[j])
                    {
                        double tempk = kk[i];
                        kk[i] = kk[j];
                        kk[j] = tempk;

                        int tempi = ii[i];
                        ii[i] = ii[j];
                        ii[j] = tempi;

                        int tempj = jj[i];
                        jj[i] = jj[j];
                        jj[j] = tempj;
                    }
                }

            for (int i = 0; i < ind; i++)
            {
                OpenList.Enqueue(state[ii[i], jj[i]]);
            }
        }//end of sortqueue
         void setstatus(Close[,] a, int hp, int wp, int gx, int gy, int sx, int sy)
        {
            for (int j = 0; j < hp; j++)
                for (int i = 0; i < wp; i++)
                {
                    if (i == gx && j == gy)
                        a[i, j].Node.status = "GOAL";
                    else if (i == sx && j == sy)
                        a[i, j].Node.status = "START";
                    else if (a[i,j].Node.node_Type==false)
                        a[i, j].Node.status = "OBSTACLE";
                    else
                        a[i, j].Node.status = "CLEAR";
                }
        }//end of set status
         void settag(Close[,] a, int hp, int wp)
        {
            for (int j = 0; j < hp; j++)
                for (int i = 0; i < wp; i++)
                    a[i, j].Node.tag = "NEW";
        }//end of settag


        //SET DIMATION VALUE OF MY NODE
         void setdimval(Close[,] a, int hp, int wp)
        {
            for (int j = 0; j < hp; j++)
                for (int i = 0; i < wp; i++)
                {
                    a[i, j].Node.x = i;
                    a[i, j].Node.y = j;
                }
        }//end of setdimval

        void initClose(Close[,] cls, Node[,] graph)
        {
            int i, j;
            for (i = 0; i < wp; i++)
            {
                for (j = 0; j < hp; j++)
                {
                    cls[i, j] = new Close { };
                    cls[i, j].Node = graph[j, i];               // Close表所指节点
                    cls[i, j].Node.isSearched = -1;// !(graph[i, j].node_Type);  // 是否被访问
                    cls[i, j].From = null;                    // 所来节点
                    cls[i, j].G = 0;  //需要根据前面的点计算
                    cls[i, j].H = 0;// Math.Abs(endX - i) + Math.Abs(endY - j);    // 评价函数值
                    cls[i, j].F = 0;  //cls[i, j].G + cls[i, j].H;
                }
            }
            //cls[endX, endY].G = AstarUtil.Infinity;     //移步花费代价值
            //cls[beginX, beginY].F = cls[beginX, beginY].H;            //起始点评价初始值
        }

        public List<MyPoint> Search(Node[,] graph, int beginX, int beginY, int endX, int endY, Direction beginDir)
        {
            List<MyPoint> route = new List<MyPoint>();
            wp = graph.GetLength(1);
            hp = graph.GetLength(0);
            this.beginX = beginX;
            this.beginY = beginY;
            this.endX = endX;
            this.endY = endY;
            Close[,] close = new Close[wp, hp];
            initClose(close, graph);
            close[beginX, beginY].Node.isSearched = 0;

            //wp = close.GetLength(0);
            // hp = close.GetLength(1);

            setdimval(close, hp, wp);
            settag(close, hp, wp);
            setstatus(close, hp, wp, endX, endY, beginX,beginY);
            insert(OpenList, close[endX,endY]);
            double kold;
            int iold, jold, ind;
            Close d = new Close();
            int[] si = new int[8];
            int[] sj = new int[8];
            

            do
            {

                d = (Close)OpenList.Dequeue();
                kold = d.G;
                iold = d.Node.x;
                jold = d.Node.y;

                close[iold, jold].Node.tag = "CLOSE";

                vs(si, sj, iold, jold);

                for (int i = 0; i < 8; i++)
                {
                    int col = si[i], row = sj[i];

                    if ((col <= wp - 1 && col >= 0) && (row <= hp - 1 && row >= 0))
                    {
                        if (col != iold && row != jold)
                            crossmove(col, row, close[col, row], close[iold, jold], OpenList);

                        else
                            linemove(col, row, close[col, row], close[iold, jold], OpenList);
                    }
                }

                ind = OpenList.Count;
                sortqueue(OpenList, close);

                String text =  "[" + iold.ToString() + "," + jold.ToString() + "] " + close[iold, jold].G.ToString() + " - ";
               // Console.WriteLine(text);

            } while (close[iold, jold].Node.status != "START");


            correnti = iold;
            correntj = jold;

            route= findpath(close, endX, endY);

            return route;
        }
         void k_min(int ni,int nj,Close s, double[] mink, int[] mi, int[] mj)
        {
           
            if (s.G < mink[0])
              {
                  mink[0] = s.G;
                  mi[0] = ni; mj[0] = nj;
                  
              }
         }//end of k_min
        private List<MyPoint> findpath(Close[,]map,int gx,int gy)
        {
            List<MyPoint> route = new List<MyPoint>();

            double[] mink = new double[1]; mink[0] = 100000;
            int[] mi = new int[1]; mi[0] = 0;
            int[] mj = new int[1]; mj[0] = 0;
            int[] neighbori = new int[8];
            int[] neighborj = new int[8];
            int[] Mi = new int[4];
            int[] Mj = new int[4];
            string[] y = new string[4];
            string[] z = new string[4];
            double pathcoast = 0.0;

            pathcoast += map[correnti, correntj].H;

            int goali = gx;
            int goalj = gy;
            bool ddd = true;
            do
            {

                vs(neighbori, neighborj, correnti, correntj);
                mink[0] = 10000;
                for (int i = 0; i < 8; i++)
                {
                    int row = neighbori[i]; int col = neighborj[i];
                    if ((row <= wp - 1 && row >= 0) && (col <= hp - 1 && col >= 0) && (map[row, col].Node.status != "OBSTACLE"))
                        k_min(row, col, map[row, col], mink, mi, mj);
                }

                int mii = mi[0]; int mjj = mj[0];


                //To Here we find the minimum k value 


                Mi[0] = correnti + 1; Mj[0] = correntj + 1;
                Mi[1] = correnti + 1; Mj[1] = correntj - 1;
                Mi[2] = correnti - 1; Mj[2] = correntj + 1;
                Mi[3] = correnti - 1; Mj[3] = correntj - 1;


                if (correntj + 1 > hp - 1)
                    z[0] = "NULL";
                else
                    z[0] = map[correnti, correntj + 1].Node.status;

                if (correntj - 1 < 0)
                    z[1] = "NULL";
                else
                    z[1] = map[correnti, correntj - 1].Node.status;

                if (correnti - 1 < 0)
                    z[2] = "NULL";
                else
                    z[2] = map[correnti - 1, correntj].Node.status;

                z[3] = z[1];


                if (correnti + 1 > wp - 1)
                    y[0] = "NULL";
                else
                    y[0] = map[correnti + 1, correntj].Node.status;

                y[1] = y[0];

                if (correntj + 1 > hp - 1)
                    y[2] = "NULL";
                else
                    y[2] = map[correnti, correntj + 1].Node.status;

                if (correnti - 1 < 0)
                    y[3] = "NULL";
                else
                    y[3] = map[correnti - 1, correntj].Node.status;


                for (int ii = 0; ii < 4; ii++)
                    if (mii == Mi[ii] && mjj == Mj[ii])
                        if (y[ii] == "OBSTACLE" && z[ii] == "OBSTACLE")
                        {

                            map[mii, mjj].H = 10000;
                            map[mii, mjj].Node.status = "OBSTACLE";
                            insert(OpenList, map[mii, mjj]);

                            int[] si = new int[8];
                            int[] sj = new int[8];
                            vs(si, sj, mii, mjj);

                            for (int i = 0; i < 8; i++)
                                if (map[si[i], sj[i]].Node.tag == "CLOSE")
                                    insert(OpenList, map[si[i], sj[i]]);
                            int iold, jold;
                            double kold;
                            bool good;
                            good = true;
                            sortqueue(OpenList, map);

                            do
                            {
                                Close d = new Close();
                                d = (Close)OpenList.Dequeue();
                                kold = d.G;
                                iold = d.Node.x;
                                jold = d.Node.y;
                                map[iold, jold].Node.tag = "CLOSE";

                                if (iold == correnti && jold == correntj)
                                    good = false;

                                vs(si, sj, iold, jold);


                                if (kold == map[iold, jold].H)
                                {
                                    for (int i = 0; i < 8; i++)
                                    {
                                        int r = si[i];
                                        int c = sj[i];

                                        if ((r <= wp - 1 && r >= 0) && (c <=hp - 1 && c >= 0))
                                        {
                                            double co = cost(iold, jold, r, c);
                                            double tt = co + map[iold, jold].H;

                                            if (map[r, c].Node.tag == "NEW")
                                            {
                                                map[r, c].From = map[iold, jold];
                                                map[r, c].H = tt;
                                                map[r, c].G = tt;
                                                insert(OpenList, map[r, c]);
                                                sortqueue(OpenList, map);
                                            }
                                            else if (((map[r, c].From == map[iold, jold]) && (map[r, c].H != tt)) || ((map[r, c].From != map[iold, jold]) && ((float)map[r, c].H > (float)tt)))
                                                if (map[r, c].Node.status != "OBSTACLE")
                                                {
                                                    map[r, c].From = map[iold, jold];
                                                    map[r, c].H = tt;

                                                    if (map[r, c].Node.tag == "CLOSE")
                                                    {
                                                        insert(OpenList, map[r, c]);
                                                        sortqueue(OpenList, map);
                                                    }
                                                }
                                        }
                                    }
                                }


                                if (kold < map[iold, jold].H)
                                {
                                    for (int i = 0; i < 8; i++)
                                    {
                                        int r = si[i];
                                        int c = sj[i];

                                        if ((r <= wp - 1 && r >= 0) && (c <=hp - 1 && c >= 0))
                                        {
                                            double co = cost(iold, jold, r, c);
                                            double tt = co + map[iold, jold].H;

                                            if (map[r, c].Node.tag == "NEW")
                                            {
                                                map[r, c].From = map[iold, jold];
                                                map[r, c].H = tt;
                                                map[r, c].G = tt;
                                                insert(OpenList, map[r, c]);
                                            }
                                            else
                                            {
                                                if ((map[r, c].From == map[iold, jold]) && (map[r, c].H != tt))
                                                {
                                                    map[r, c].From = map[iold, jold];
                                                    if (map[r, c].Node.tag == "CLOSE")
                                                    {
                                                        if (map[iold, jold].H == 10000)
                                                            map[r, c].H = 10000;
                                                        insert(OpenList, map[r, c]);
                                                        sortqueue(OpenList, map);
                                                    }

                                                    else
                                                    {
                                                        map[r, c].H = tt;
                                                        if (map[r, c].H > 10000)
                                                            map[r, c].H = 10000;
                                                    }
                                                }
                                                else
                                                {
                                                    if ((map[r, c].From != map[iold, jold]) && (map[r, c].H > tt))
                                                    {
                                                        map[iold, jold].G = tt;
                                                        insert(OpenList, map[iold, jold]);
                                                        sortqueue(OpenList, map);
                                                    }
                                                    else
                                                    {
                                                        if (map[r, c].From != map[iold, jold])
                                                        {
                                                            if (map[iold, jold].H > map[r, c].H + cost(iold, jold, r, c))
                                                            {
                                                                if (map[r, c].Node.tag == "CLOSE")
                                                                {
                                                                    if (map[r, c].H > kold)
                                                                    {
                                                                        insert(OpenList, map[r, c]);
                                                                        sortqueue(OpenList, map);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            } while (good);



                            bool end = true;

                            while (end)
                            {
                                Close d = new Close();
                                d = (Close)OpenList.Dequeue();
                                kold = d.G;
                                iold = d.Node.x;
                                jold = d.Node.y;
                                map[iold, jold].Node.tag = "CLOSE";

                                if (kold < map[correnti, correntj].H)
                                {

                                    vs(si, sj, iold, jold);

                                    if (kold == map[iold, jold].H)
                                    {
                                        for (int i = 0; i < 8; i++)
                                        {
                                            int r = si[i];
                                            int c = sj[i];

                                            if ((r <= wp - 1 && r >= 0) && (c <= hp - 1 && c >= 0))
                                            {
                                                double co = cost(iold, jold, r, c);
                                                double tt = map[iold, jold].H + co;

                                                if (map[r, c].Node.tag == "NEW")
                                                {
                                                    map[r, c].From = map[iold, jold];
                                                    map[r, c].H = map[iold, jold].H + co;
                                                    map[r, c].G = map[r, c].H;
                                                    insert(OpenList, map[r, c]);
                                                    sortqueue(OpenList, map);
                                                }
                                                else if (((map[r, c].From == map[iold, jold]) && ((float)map[r, c].H != (float)tt)) || ((map[r, c].From != map[iold, jold]) && ((float)map[r, c].H > (float)tt)))
                                                {
                                                    if (map[r, c].Node.status != "OBSTACLE")
                                                    {
                                                        map[r, c].From = map[iold, jold];
                                                        map[r, c].H = map[iold, jold].H + co;
                                                        map[r, c].G = map[r, c].H;
                                                        if (map[r, c].Node.tag == "CLOSE")
                                                        {
                                                            insert(OpenList, map[r, c]);
                                                            sortqueue(OpenList, map);
                                                        }
                                                    }
                                                }

                                            }
                                        }

                                    }


                                    else if (kold < map[iold, jold].H)
                                    {
                                        for (int i = 0; i < 8; i++)
                                        {
                                            int r = si[i];
                                            int c = sj[i];

                                            if ((r <= 4 && r >= 0) && (c <= 4 && c >= 0))
                                            {
                                                double co = cost(iold, jold, r, c);
                                                double tt = map[iold, jold].H + co;

                                                if (map[r, c].Node.tag == "NEW")
                                                {
                                                    map[r, c].From = map[iold, jold];
                                                    map[r, c].H = tt;
                                                    map[r, c].G = tt;
                                                    insert(OpenList, map[r, c]);
                                                    sortqueue(OpenList, map);
                                                }
                                                else if ((map[r, c].From == map[iold, jold]) && ((float)map[r, c].H != (float)tt))
                                                {
                                                    map[r, c].From = map[iold, jold];
                                                    if (map[r, c].Node.tag == "CLOSE")
                                                    {
                                                        map[r, c].H = tt;
                                                        if (map[r, c].H > 10000)
                                                            map[r, c].H = 10000;
                                                        insert(OpenList, map[r, c]);
                                                        sortqueue(OpenList, map);
                                                    }

                                                }
                                                else if ((map[r, c].From != map[iold, jold]) && ((float)map[r, c].H > (float)tt))
                                                {
                                                    if (map[iold, jold].Node.tag == "CLOSE")
                                                    {
                                                        map[iold, jold].G = map[iold, jold].H;
                                                        insert(OpenList, map[iold, jold]);
                                                        sortqueue(OpenList, map);
                                                    }
                                                }
                                                else if ((map[r, c].From != map[iold, jold]) && ((float)map[iold, jold].H > (float)(map[r, c].H + co)) && (map[r, c].Node.tag == "CLOSE") && (map[r, c].H > (float)kold))
                                                {
                                                    insert(OpenList, map[r, c]);
                                                    sortqueue(OpenList, map);
                                                }

                                            }
                                        }
                                    }


                                }
                                else
                                {
                                    end = false;
                                    OpenList.Enqueue(map[iold, jold]);
                                    sortqueue(OpenList, map);
                                }

                            }

                            mii = correnti;
                            mjj = correntj;

                        }
                correnti = mii;
                correntj = mjj;
                if ((correnti == goali) && (correntj == goalj))
                    ddd = false;
                
                // Form1.ActiveForm.Refresh();
                pathcoast += map[correnti, correntj].H;
                route.Add(new MyPoint(correnti, correntj,Direction.Left));

                Console.Write("(" + correnti.ToString() + ", " + correntj.ToString() + ") - ");

            } while (ddd);
            Console.WriteLine();
            return route;

        }
         double cost(int x1, int y1, int x2, int y2)
        {
            if ((x2 - x1 == 0) || (y2 - y1 == 0))
            {
                return (1.0);
            }
            return (1.4);
        }//end cost
               
    }
}
