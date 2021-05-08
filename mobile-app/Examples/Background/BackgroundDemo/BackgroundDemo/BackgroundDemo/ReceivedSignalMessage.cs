using System;
using System.Collections.Generic;
using System.Text;

namespace BackgroundDemo
{
    public class ReceivedSignalMessage
    {
        private string message;
        public ReceivedSignalMessage(string mess)
        {
            message = mess;
        }

        public string getMessage()
        {
            return message;
        }
    }
}
