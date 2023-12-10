using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorTRPO
{
    internal interface IMemory
    {
        void WriteSomewhere(string number);
        string ReadSomewhere();
    }
}
