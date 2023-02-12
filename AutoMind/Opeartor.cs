namespace AutoMind
{
    public abstract class Operartor : FormulaElement
    {
        public List<FormulaElement> Arguments;
        public static new readonly string Type = "OPERATOR";
        public virtual string Name => "BASE";
        public abstract Operartor ExpressForm(FormulaElement argument, FormulaElement anotherSight);
        public static List<Property> GetPropertiesInside(Operartor opeartor)
        {
            var ret = new List<Property>();
            foreach (var argument in opeartor.Arguments)
            {
                if (argument is Operartor)
                    ret.AddRange(Operartor.GetPropertiesInside(argument as Operartor));
                else if (argument is Property)
                    ret.Add(argument as Property);
            }
            return ret;
        }
        public Operartor()
        {
            Arguments = new List<FormulaElement>();
        }
        public Operartor(List<FormulaElement> arguments)
        {
            Arguments = arguments;
        }
        public override string ToString()
        {
            return $"OP.{Name}[{string.Join(";", Arguments.Select(i => i.ToString()))}]";
        }
    }
    public class Addition : Operartor
    {
        public override string Name => "ADD";

        public Addition() : base() { }

        public Addition(List<FormulaElement> arguments) : base(arguments)
        {
        }

        public override Operartor ExpressForm(FormulaElement argument, FormulaElement anotherSight)
        {
            FormulaElement fe = null;
            if (Arguments.Count == 2)
                fe = Arguments.FirstOrDefault(i => i != argument);
            else
                fe = new Addition(Arguments.Where(i => i != argument).ToList());
            return new Subtraction(new List<FormulaElement> { anotherSight, fe });
        }
    }
    public class Subtraction : Operartor
    {
        public override string Name => "SUB";
        public Subtraction() : base() { }
        public Subtraction(List<FormulaElement> arguments) : base(arguments)
        {
        }

        public override Operartor ExpressForm(FormulaElement argument, FormulaElement anotherSight)
        {
            var i = Arguments.IndexOf(argument);
            switch (i)
            {
                case 0:
                    return new Addition(new List<FormulaElement> { Arguments[1], anotherSight });
                case 1:
                    return new Subtraction(new List<FormulaElement> { Arguments[0], anotherSight });

                default:
                    return null;
            }
        }
    }
    public class Multiplication : Operartor
    {
        public override string Name => "MUL";
        public Multiplication() : base() { }

        public Multiplication(List<FormulaElement> arguments) : base(arguments)
        {
        }

        public override Operartor ExpressForm(FormulaElement argument, FormulaElement anotherSight)
        {
            FormulaElement fe = null;
            if (Arguments.Count == 2)
                fe = Arguments.FirstOrDefault(i => i != argument);
            else
                fe = new Multiplication(Arguments.Where(i => i != argument).ToList());
            return new Division(new List<FormulaElement> { anotherSight, fe });
        }
    }
    public class Division : Operartor
    {
        public override string Name => "DIV";
        public Division() : base() { }

        public Division(List<FormulaElement> arguments) : base(arguments)
        {
        }

        public override Operartor ExpressForm(FormulaElement argument, FormulaElement anotherSight)
        {
            var i = Arguments.IndexOf(argument);
            switch (i)
            {
                case 0:
                    return new Multiplication(new List<FormulaElement> { Arguments[1], anotherSight });
                case 1:
                    return new Division(new List<FormulaElement> { Arguments[0], anotherSight });

                default:
                    return null;
            }
        }
    }
    public class Neganive : Operartor
    {
        public override string Name => "NEG";
        public Neganive(List<FormulaElement> args) : base(args) { }
        public override Operartor ExpressForm(FormulaElement argument, FormulaElement anotherSight)
        {
            return new Neganive(new List<FormulaElement> { anotherSight });
        }
    }
}
