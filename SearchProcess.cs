using Agv.PathPlanning;
using AGV_V1._0.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGV_V1._0
{
    partial class SearchProcess : Form
    {
        
        //public static readonly int FORM_WIDTH = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;   //框体的宽度
        //public static readonly int FORM_HEIGHT = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;    //框体的长度  

        private Bitmap surface = null;
        private Graphics g = null;
        private static Node[,] graph;
        private int nodeLength = 10;
        private static int startX;
        private static int startY;
        private static int endX;
        private static int endY;
        private int width;
        private int height;
        private static List<MyPoint> route = new List<MyPoint>();

        public SearchProcess()
        {
            InitializeComponent();
            

        }
        public static void SetGraph(Node[,] g,List<MyPoint>r, int startX1, int startY1, int endX1, int endY1)
        {
            startX = startX1;
            startY = startY1;
            endX = endX1;
            endY = endY1;
            graph = g;
            route = r;
        }
        void LoadView()
        {
            if (graph == null)
            {
                return;
            }
            // this.WindowState = FormWindowState.Maximized;
            nodeLength = (int)(this.Size.Width) /2/ (graph.GetLength(0) + 1);

            SetMapView();
            // SetInfoShowView();
        }
        void SetMapView()
        {


            height =  (graph.GetLength(0));
            width = (graph.GetLength(1));
            int w = width * nodeLength;
            int h = height * nodeLength;
            //设置pictureBox的尺寸和位置
            pic.Location = Point.Empty;
            pic.Size = new Size( w,h);
            surface = new Bitmap(w,h);
            g = Graphics.FromImage(surface);
            //将pictureBox加入到panel上
            
            pic.BackColor = Color.FromArgb(100, 0, 0, 0);
            DrawMap();
           
        }
        void DrawMap()
        {

            if (graph == null)
            {
                return;
            }

            //节点类型
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                   {
                
                    //drawArrow(i, j);
                    //绘制表格
                    int point_x = j*nodeLength-1;
                    int point_y = i*nodeLength-1;

                    if (graph[i, j].node_Type)
                    {
                        DrawUtil.FillRectangle(g, Color.LightGray, point_x,point_y, nodeLength - 2, nodeLength - 2);
                    }
                    else
                    {
                        DrawUtil.FillRectangle(g, Color.Black, point_x, point_y, nodeLength - 2, nodeLength - 2);
                    }

                    //绘制标尺
                    if (i == 0 || i == height - 1)
                    {
                        DrawUtil.FillRectangle(g, Color.FromArgb(180, 0, 0, 0), point_x, point_y, nodeLength - 2, nodeLength - 2);
                        DrawUtil.DrawString(g, j, nodeLength / 2, Color.Yellow, point_x, point_y);
                    }
                    if (j == 0 || j == width- 1)
                    {
                        DrawUtil.FillRectangle(g, Color.FromArgb(180, 0, 0, 0), point_x, point_y, nodeLength - 2, nodeLength - 2);
                        DrawUtil.DrawString(g, i, nodeLength / 2, Color.Yellow, point_x, point_y);
                    }

                    if (graph[i, j].isSearched>=0)
                    {
                        DrawUtil.FillRectangle(g, Color.FromArgb(180, 0, graph[i,j].isSearched, 0), point_x, point_y, nodeLength - 2, nodeLength - 2);
                        DrawUtil.DrawString(g, graph[i,j].isSearched, 9,Color.White, point_x, point_y);

                    }
                    

                }
            }
            int colorR = 250;
            if (route != null)
            {
                for (int i = 0; i < route.Count; i++)
                {
                    colorR -= 10;
                    if (colorR < 25)
                    {
                        colorR = 25;
                    }
                    DrawUtil.FillRectangle(g, Color.FromArgb(180, colorR, 0, 0), route[i].Y * nodeLength, route[i].X * nodeLength, nodeLength - 2, nodeLength - 2);
                    
                }
            }

            DrawUtil.FillEllipse(g, Color.Blue, startX*nodeLength, startY*nodeLength, nodeLength - 2, nodeLength - 2);
            DrawUtil.FillEllipse(g, Color.FromArgb(180, 255, 0, 0), endX*nodeLength, endY*nodeLength, nodeLength - 2, nodeLength - 2);

            pic.Image = surface;

        }

        private void SearchProcess_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadView();
        }
    }
}
