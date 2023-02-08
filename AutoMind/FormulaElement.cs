using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMind
{
    public abstract class FormulaElement
    {
        public static readonly string Type = "NONE";
        public string DataType => GetType().GetField("Type").GetValue(this) as string;
        public override string ToString()
        {
            return DataType;
        }
        public abstract List<Property> GetInside();
    }
}
