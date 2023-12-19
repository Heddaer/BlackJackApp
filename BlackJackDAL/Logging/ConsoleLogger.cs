using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace BlackJackDAL.Logging
{
    public class ConsoleLogger
    {
        public void LogToConsole(string message)
        {
            Console.WriteLine(message);
        }
    }
}
