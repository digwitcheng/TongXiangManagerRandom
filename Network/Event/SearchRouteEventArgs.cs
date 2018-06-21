using System;

namespace AGV_V1._0.Event
{
    class SearchRouteEventArgs: EventArgs
    {
        public SendData sendData { get; set; }
        public SearchRouteEventArgs(SendData td)
        {
            this.sendData = td;
        }
    }
}
