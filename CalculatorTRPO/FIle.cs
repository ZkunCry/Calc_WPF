using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorTRPO
{
    internal class FIle : IMemory
    {
        private readonly string _nameFile;
        public FIle(string nameFile = "memory.txt")
        {
            _nameFile = nameFile;
        }
        public  string ReadSomewhere()
        {
            string? line;

            using (StreamReader writer = new StreamReader(_nameFile, false))
            {
                line = writer.ReadLine();
            
            }
            return line;
        }

        public async void WriteSomewhere(string number)
        {
            using (StreamWriter writer = new StreamWriter(_nameFile, false))
            {
                await writer.WriteAsync(number);
            }
        }
    }
}
