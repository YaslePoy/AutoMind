using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoMind
{
    public abstract class FormulaElement
    {
        public Pack Origin;
        public static readonly string Type = "NONE";
        public string DataType => GetType().GetField("Type").GetValue(this) as string;
        public override string ToString()
        {
            return DataType;
        }

        public abstract double GetValue();
        public abstract string ToView();
        public abstract string ToValue();
        public static FormulaElement ParceElement(string element, CalculatingEnvironment environment, Pack origin)
        {
            if (element.IsNumeric())
                return new Number() { Value = Utils.ParceDouble(element) };
            if (element.StartsWith("OP"))
            {
                var opName = element.Substring(3, element.IndexOf("[") - 3);
                var args = element.Substring(element.IndexOf("[") + 1);
                args = args.Substring(0, args.Length - 1);
                var split = Utils.SplitMultyMarker(args);
                List<FormulaElement> argsE = new List<FormulaElement>();
                foreach (var arg in split)
                {
                    argsE.Add(FormulaElement.ParceElement(arg, environment, origin));
                }
                var opType = CalculatingEnvironment.Operators.FirstOrDefault(i => i.Name == opName).GetType();
                var opInstance = Activator.CreateInstance(opType, argsE) as Operartor;
                return opInstance as Operartor;
            }
            if (element.Contains("PR"))
            {
                return environment.GetProperty(element, origin);
            }
            if (element.Contains("CN"))
            {
                return environment.GetConstant(element, origin);
            }
            string head = element.Substring(0, 2);
            string body = element.Substring(3);
            if (head == "OP")
            {

            }
            return null;
        }
    }
}
