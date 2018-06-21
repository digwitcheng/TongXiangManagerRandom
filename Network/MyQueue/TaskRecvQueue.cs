namespace AGV_V1._0.Queue
{
    class TaskRecvQueue : BaseQueue<string>
    {
        private static TaskRecvQueue instance;
        public static TaskRecvQueue Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new TaskRecvQueue();
                }
                return instance;
            }
        }
    }
}
