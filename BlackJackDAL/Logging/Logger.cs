// Author: Hedda Eriksson
// Date: 2023-10-17
// Malmö University, Bachelor of Engineering - Computer Science & Telecommunications
// Description: Logger to log actions.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackDAL.Logging
{
    public class Logger
    {
        public Action<string> LogMessage;

        public Logger(Action<string> action)
        {
            LogMessage = action;
        }

        public void LogAction(string action, string description)
        {
            string logmessage = action + " " + description;
            LogMessage(logmessage);
        }
    }
}
