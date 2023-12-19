// Author: Hedda Eriksson
// Date: 2023-10-11
// Malmö University, Bachelor of Engineering - Computer Science & Telecommunications
// Description: for multiuse when converting strings.
namespace UtilityLib.Utilites
{
    public class StringConverter
    {
        /// <summary>
        /// Convert string to integer.
        /// </summary>
        /// <param name="str">The input string to be converted to an integer.</param>
        /// <returns>The integer value</returns>
        public static int StringToInteger(string str, out string errorMsg)
        {
            int result;
            errorMsg = string.Empty;

            if (int.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                errorMsg = "Invalied integer format";
                return 0;
            }
        }

        /// <summary>
        /// Convert string to decimal.
        /// </summary>
        /// <param name="str">The input string to be converted to a decimal.</param>
        /// <returns>The decimal value</returns>
        public static decimal StringToDecimal(string str, out string errorMsg)
        {
            decimal result;
            errorMsg = string.Empty;
            if(decimal.TryParse(str, out result)) { return result; }
            else { errorMsg = "Invalied decimal format";  return 0; }
        }

        /// <summary>
        /// Converts a string to an integer and checks if it is within a specified range.
        /// </summary>
        /// <param name="str">The input string to be converted to an integer.</param>
        /// <param name="lowLimit">The lower limit of the allowed range.</param>
        /// <param name="highLimit">The higer limit of the allowed range</param>
        /// <returns>The integer value if it is within specified range</returns>
        public static int StringToInteger(string str, int lowLimit, int highLimit, out string errorMsg)
        {
            int result = StringToInteger(str, out errorMsg);
          
            if (result < lowLimit || result > highLimit)
            {
                errorMsg = "Value is outside the specified range";
            }
            return result;
        }

        /// <summary>
        /// Converts a string to a decimal and checks if it is within a specified range.
        /// </summary>
        /// <param name="str">The input string to be converted to a decimal.</param>
        /// <param name="lowLimit">The lower limit of the allowed range.</param>
        /// <param name="highLimit">The higer limit of the allowed range</param>
        /// <returns>The decimal value if it is within specified range</returns>
        public static decimal StringToDecimal(string str, int lowLimit, int highLimit, out string errorMsg)
        {
            decimal result = StringToDecimal(str, out errorMsg); 
            
            if(result < lowLimit || result > highLimit) { errorMsg = "Value is outside the specified range"; }
            return result;
        }
    }
}