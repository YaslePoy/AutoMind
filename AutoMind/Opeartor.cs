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

        public override string ToView()
        {
            return "(" + string.Join(" + ", Arguments.Select(i => i.ToView())) + ")";
        }
        public override string ToValue()
        {
            return "(" + string.Join(" + ", Arguments.Select(i => i.ToValue())) + ")";
        }

        public override double GetValue()
        {
            return Arguments.Sum(i => i.GetValue());
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

        public override string ToView()
        {
            return "(" + string.Join(" - ", Arguments.Select(i => i.ToView())) + ")";
        }
        public override string ToValue()
        {
            return "(" + string.Join(" - ", Arguments.Select(i => i.ToValue())) + ")";

        }
        public override double GetValue()
        {
            return Arguments[0].GetValue() - Arguments[1].GetValue();
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

        public override string ToView()
        {
            return "(" + string.Join(" * ", Arguments.Select(i => i.ToView())) + ")";
        }
        public override string ToValue()
        {
            return "(" + string.Join(" * ", Arguments.Select(i => i.ToValue())) + ")";

        }
        public override double GetValue()
        {
            double ret = 1;
            Arguments.ForEach(i => ret *= i.GetValue());
            return ret;
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

        public override string ToView()
        {
            return "(" + string.Join(" / ", Arguments.Select(i => i.ToView())) + ")";
        }
        public override string ToValue()
        {
            return "(" + string.Join(" / ", Arguments.Select(i => i.ToValue())) + ")";

        }
        public override double GetValue()
        {
            return Arguments[0].GetValue() / Arguments[1].GetValue();
        }
    }
    public class Negative : Operartor
    {
        public override string Name => "NEG";
        public Negative() : base() { }

        public Negative(List<FormulaElement> args) : base(args) { }
        public override Operartor ExpressForm(FormulaElement argument, FormulaElement anotherSight)
        {
            return new Negative(new List<FormulaElement> { anotherSight });
        }

        public override string ToView()
        {
            return "-" + Arguments[0].ToView();
        }
        public override string ToValue()
        {
            return "-" + Arguments[0].ToValue();

        }
        public override double GetValue()
        {
            return -Arguments[0].GetValue();
        }
    }
    public class Square : Operartor
    {
        public override string Name => "SQ";
        public Square() : base() { }

        public Square(List<FormulaElement> arguments) : base(arguments)
        {
        }

        public override Operartor ExpressForm(FormulaElement argument, FormulaElement anotherSight)
        {
            return new Root(new List<FormulaElement> { anotherSight });
        }

        public override string ToView()
        {
            return $"{Arguments[0].ToView()}^2";
        }
        public override string ToValue()
        {
            return $"{Arguments[0].ToValue()}^2";

        }
        public override double GetValue()
        {
            return Math.Pow(Arguments[0].GetValue(), 2);
        }
    }
    public class Root : Operartor
    {
        public override string Name => "RT";
        public Root() : base() { }

        public Root(List<FormulaElement> arguments) : base(arguments)
        {
        }

        public override Operartor ExpressForm(FormulaElement argument, FormulaElement anotherSight)
        {
            return new Square(new List<FormulaElement> { anotherSight });
        }

        public override string ToView()
        {
            return $"2↓{Arguments[0].ToView()}";
        }
        public override string ToValue()
        {
            return $"2↓{Arguments[0].ToValue()}";

        }
        public override double GetValue()
        {
            return Math.Sqrt(Arguments[0].GetValue());
        }
    }

}
