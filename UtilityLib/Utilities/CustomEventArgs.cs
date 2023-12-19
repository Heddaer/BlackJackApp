using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityLib.Utilities
{
    public class CustomEventArgs : EventArgs
    {
        public int PlayerIndex { get; set; }
        public string? Card { get; set; }
        public int CardNbr { get; set; }

    }
}
