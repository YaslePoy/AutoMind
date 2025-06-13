using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMind
{
    public class Constant : Number
    {
        public override string DataType => "CONST";
        public string Name;
        public string View;

        public override string ToString()
        {
            return $"CN.{View}";
        }
        public override string ToView()
        {
            return View;
        }

        public string NameView => Name.Replace('_', ' ');
    }

}
