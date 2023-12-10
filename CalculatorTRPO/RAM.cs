using System;


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
