using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMind
{
    public class Formula
    {
        public FormulaElement Expression;
        public FormulaElement Head;
        public List<Property> TotalProperties
        {
            get
            {
                var total = new List<Property>();

            }
        }
    }
}
