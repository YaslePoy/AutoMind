using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoMind
{
    public abstract class Opeartor : FormulaElement
    {
        public List<FormulaElement> Arguments;
        public static new readonly string Type = "OPERATOR";
        public static readonly string Name = "BASE";
        public abstract Opeartor ExpressForm(FormulaElement agrument);
        public static List<Property> GetPropertiesInside(Opeartor opeartor)
        {
            var ret = new List<Property>();
            foreach (var argument in opeartor.Arguments)
            {
                if (argument.DataType == "OPERATOR")
                    ret.AddRange(Opeartor.GetPropertiesInside(argument as Opeartor));
                else if (argument.DataType == "PROP")
                    ret.Add(argument as Property);
            }
            return ret;
        }
        public Opeartor(List<FormulaElement> arguments)
        {
            Arguments = arguments;
        }
    }
}
