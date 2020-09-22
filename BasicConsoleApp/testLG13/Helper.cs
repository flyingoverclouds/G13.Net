using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G13GamingKeyboard
{
    static class Helper
    {
        /// <summary>
        /// return the binary representation of a byte
        /// </summary>
        /// <param name="b">byte</param>
        /// <returns>binary representation of the byte</returns>
        public static string ToBinaryString(this byte b)
        {
            StringBuilder sb = new StringBuilder(8);
            for (int n = 0; n < 8; n++)
            {
                if ((b & 128) != 0)
                    sb.Append("1");
                else sb.Append("0");
                b = (byte)(b << 1);
            }
            return sb.ToString();
        }
    }
}
