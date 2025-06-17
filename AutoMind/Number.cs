namespace AutoMind
{
    public class Number : FormulaElement
    {
        public override string DataType => "NUMBER";
        public double Value { get; set; } = 1;
        public override double GetValue()
        {
            return Value;
        }
        public override string ToString()
        {
            return Value.ToString();
        }

        public override string ToView()
        {
            return Value.ToString();
        }
        public override string ToValue()
        {
            return Value.ToString();
        }
    }
}
