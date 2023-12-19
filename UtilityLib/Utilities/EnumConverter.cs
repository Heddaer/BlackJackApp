// Author: Hedda Eriksson
// Date: 2023-10-17
// Malmö University, Bachelor of Engineering - Computer Science & Telecommunications
// Description: Converts enum.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UtilityLib.Utilities
{
    public class EnumConverter
    {
        public static int EnumToInteger(Enum number)
        {
            switch (number.ToString())
            {
                case "One":
                    return 1;
                case "Two":
                    return 2;
                case "Three":
                    return 3;
                case "Four":
                    return 4;
                case "Five":
                    return 5;
                case "Six":
                    return 6;
                case "Seven":
                    return 7;
                case "Eight":
                    return 8;
                case "Nine":
                    return 9;
                case "Ten":
                    return 10;
                default:
                    throw new ArgumentException("Invalid enum value");
                   
            }
        }
    }
}
