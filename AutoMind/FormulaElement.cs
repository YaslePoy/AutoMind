using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public static FormulaElement ParceElement(string element, CalculatingEnvironment environment)
        {
            if (element.IsNumeric())
                return new Number() { Value = Utils.ParceDouble(element) };
            string head = element.Substring(0, 2);
            string body = element.Substring(3);
            if (head == "PR")
            {
                return environment.Properties.FirstOrDefault(i => i.View == body);
            }
            if (head == "CN")
            {
                var name = element.Substring(3);
                return environment.Constants.FirstOrDefault(i => i.View == body);
            }
            if (head == "OP")
            {
                var opName = body.Substring(0, body.IndexOf("["));
                var args = body.Substring(body.IndexOf("[") + 1);
                args = args.Substring(0, args.Length - 1);
                var split = Utils.SplitMultyMarker(args);
                List<FormulaElement> argsE = new List<FormulaElement>();
                foreach (var arg in split)
                {
                    argsE.Add(FormulaElement.ParceElement(arg, environment));
                }
                var flsd = CalculatingEnvironment.Operators[0].GetField("Name", BindingFlags.Static | BindingFlags.Public);
                var o = flsd.GetValue(null);
                var opType = CalculatingEnvironment.Operators.FirstOrDefault(i =>
                {
                    var field = i.GetField("Name", BindingFlags.Static | BindingFlags.Public);
                    var nm = field.GetValue(null) as string;
                    return nm == opName;
                });
                var opInstance = Activator.CreateInstance(opType, argsE) as Opeartor;
                return opInstance as Opeartor;
            }
            return null;
        }
    }
}
