namespace AGV_V1._0.Queue
{
    class FinishedQueue:BaseQueue<Vehicle>
    {
        private static FinishedQueue instance;
        public static FinishedQueue Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new FinishedQueue();
                }
                return instance;
            }
        }
    }
}
