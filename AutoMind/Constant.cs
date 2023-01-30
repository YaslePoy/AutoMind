using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMind
{
    internal class Constant : Number
    {
        public new string Type = "CONST";
        public string Name;
        public string View;
    }
}
