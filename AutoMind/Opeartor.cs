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
        public new string Type = "Operator";
        public string Name;
        public abstract Opeartor ExpressForm(FormulaElement agrument);

    }
}
