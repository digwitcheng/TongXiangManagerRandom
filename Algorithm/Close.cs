namespace Agv.PathPlanning
{
    class Close
    {
       private  Node node;

        internal Node Node
        {
            get { return node; } 
            set { node = value; } 
        }
      
       private   Close from;

        internal Close From
        {
            get { return from; }
            set { from = value; }
        }
        private  double f,g,h;  

        public double G
        {
            get { return g; }
            set { g = value; }
        }

        public double F
        {
            get { return f; }
            set { f = value; }
        }
       public double H
        {
            get { return h; }
            set { h = value; }
        }

      
    }
}
