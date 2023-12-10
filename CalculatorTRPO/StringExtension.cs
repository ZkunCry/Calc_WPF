using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorTRPO
{
    public static class StringExtension
    {
        public static string Pop_Back(this string str) =>
            str.Length == 1 ? "0" : str.Remove(str.Length - 1);
    }
}
