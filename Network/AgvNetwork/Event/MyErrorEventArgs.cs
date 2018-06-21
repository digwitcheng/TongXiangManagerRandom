﻿using System;

namespace AGV.Event
{
    public class MyErrorEventArgs : EventArgs
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public MyErrorEventArgs(string message)
            : base()
        {
            this.Message = message;
        }
        public MyErrorEventArgs(string message, Exception exception)
            : base()
        {
            this.Message = message;
            this.Exception = exception;
        }
    }
}
