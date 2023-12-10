using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CalculatorTRPO
{
    internal class RAM : IMemory
    {
        private string memoryNum;
        public RAM()
        {
            memoryNum = "0";
        }
        public string ReadSomewhere()
        {
           return memoryNum;
        }

        public void WriteSomewhere(string number)
        {
            memoryNum = number;
        }
    }
}
