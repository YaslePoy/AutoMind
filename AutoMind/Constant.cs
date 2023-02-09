using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMind
{
    public class Constant : Number
    {
        public static new readonly string Type = "CONST";
        public string Name;
        public string View;

        public override string ToString()
        {
            return $"CN.{Name}";
        }
    }
}
