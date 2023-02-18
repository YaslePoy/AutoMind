using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMind
{
    public class Number : FormulaElement
    {
        public static new string Type = "NUMBER";
        public double Value = 1;
        public override double GetValue()
        {
            return Value;
        }
        public override string ToString()
        {
            return Value.ToString();
        }

        public override string ToView()
        {
            return Value.ToString();
        }
        public override string ToValue()
        {
            return Value.ToString();
        }
    }
}
