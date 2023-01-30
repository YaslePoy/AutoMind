using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMind
{
    internal class Number : FormulaElement
    {
        public static readonly new string Type = "NUMBER";
        public double Value;
    }
}
