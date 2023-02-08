using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMind
{
    internal abstract class Opeartor : FormulaElement
    {
        public List<FormulaElement> Arguments;
        public static new readonly string Type = "OPERATOR";
        public string Name;
        public abstract Opeartor ExpressForm(FormulaElement agrument);

    }
}
