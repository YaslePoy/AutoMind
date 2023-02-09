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
        public Opeartor()
        {
            Arguments = new List<FormulaElement>();
        }
        public Opeartor(List<FormulaElement> arguments)
        {
            Arguments = arguments;
        }
        public override string ToString()
        {
            return $"OP.{Name}[{string.Join(";", Arguments.Select(i => i.ToString()))}]";
        }
    }
    public class Addition : Opeartor
    {
        public static new readonly string Name = "ADD";

        public Addition(List<FormulaElement> arguments) : base(arguments)
        {
        }

        public override Opeartor ExpressForm(FormulaElement agrument)
        {
            throw new NotImplementedException();
        }
    }
    public class Subtraction : Opeartor
    {
        public static new readonly string Name = "SUB";

        public Subtraction(List<FormulaElement> arguments) : base(arguments)
        {
        }

        public override Opeartor ExpressForm(FormulaElement agrument)
        {
            throw new NotImplementedException();
        }
    }
    public class Multiplication : Opeartor
    {
        public static new readonly string Name = "MUL";

        public Multiplication(List<FormulaElement> arguments) : base(arguments)
        {
        }

        public override Opeartor ExpressForm(FormulaElement agrument)
        {
            throw new NotImplementedException();
        }
    }
    public class Division : Opeartor
    {
        public static new readonly string Name = "DIV";

        public Division(List<FormulaElement> arguments) : base(arguments)
        {
        }

        public override Opeartor ExpressForm(FormulaElement agrument)
        {
            throw new NotImplementedException();
        }
    }
}
