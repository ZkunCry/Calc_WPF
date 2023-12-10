using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorTRPO
{
    public class Parser
    {
        private string _TemplateString;
        private bool isInvalid = false;
        private int symbol, indexsrc = 0;
        private void GetSymbol()
        {
            if (indexsrc + 1 <= _TemplateString.Length)
            {
                symbol = _TemplateString[indexsrc];
                indexsrc++;
            }
            else
                symbol = '\0';
        }

        public string StartParsing(string SourceStr)
        {
            _TemplateString = SourceStr;
          
            if (string.IsNullOrWhiteSpace(_TemplateString))
                throw new ArgumentNullException("String is null or contains whitespace");
            indexsrc = 0;
            GetSymbol();
            string result = MethodE().ToString();
            if (isInvalid)
            {
                isInvalid = false;
                return "Error";
            }
            else
                return result;
        }
        private double MethodE()
        {
            double x = MethodT();
            while (symbol == '+' || symbol == '-')
            {
                char p = (char)symbol;
                GetSymbol();
                if (p == '+')
                    x += MethodT();
                else
                    x -= MethodT();
            }
            return x;
        }
        private double MethodT()
        {
            double x = MethodM();
            while (symbol == '*' || symbol == '/')
            {
                char p = (char)symbol;
                GetSymbol();
                if (p == '*')
                    x *= MethodM();
                else
                {
                    var result = MethodM();
                    if (result == 0)
                    {
                        isInvalid = true;

                        return 0;

                    }
                    x /= result;
                }
            }
            return x;
        }
        private double MethodM()
        {
            double x = 0;
            if (symbol == '(')
            {
                GetSymbol();
                x = MethodE();
                if (symbol != ')')
                {
                    isInvalid = true;
                    return 0;
                }
                GetSymbol();
                if (symbol >= '0' && symbol <= '9')
                {
                    isInvalid = true;
                    return 0;
                }
            }
            else
            {
                if (symbol == '-')
                {
                    GetSymbol();
                    x = -MethodM();
                }
                else if (symbol >= '0' && symbol <= '9')
                    x = MethodC();
                else if (symbol >= 'a' && symbol <= 'z')
                {
                    throw new ArgumentException($"Invalid parameter. Expected: 1+1,but current string:{_TemplateString} ");

                }
                else if (symbol == '(' || symbol == ')')
                {
                    isInvalid = true;
                    return 0;
                }

            }
            return x;
        }
        private double MethodC()
        {
            string x = "";
            bool isDot = false;
            double temp2 = 0;
            char temp = (char)symbol;
            while (symbol >= '0' && symbol <= '9')
            {

                x += (char)symbol;
                GetSymbol();
                if (symbol == '(')
                {
                    temp2 = MethodE();
                    if (symbol != ')')
                    {
                        isInvalid = true;
                        return 0;
                    }
                    GetSymbol();
                }
                if (symbol == ',')
                {
                    if (isDot)
                    {
                        isInvalid = true;
                        return 0;
                    }
                    x += ',';
                    GetSymbol();
                    isDot = true;
                }

            }
            return double.Parse(x);
        }

    }
}
