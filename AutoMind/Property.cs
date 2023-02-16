using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMind
{
    public class Property : Constant
    {
        public static new readonly string Type = "PROP";
        public readonly string UnitsFull;
        public readonly string UnitsShort;
        public string UnitsView => UnitsFull.Replace('_', ' ');
    }
}
