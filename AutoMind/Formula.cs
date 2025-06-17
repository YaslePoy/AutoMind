using System.Text.Json.Serialization;

namespace AutoMind
{
    public class Formula
    {
        public string RawHead { get; set; }
        public string RawExpression { get; set; }

        [JsonIgnore]
        public FormulaElement Expression { get; set; }

        [JsonIgnore]
        public FormulaElement Head { get; set; }

        [JsonIgnore]
        public Pack Origin { get; set; }

        public List<Property> TotalProperties
        {
            get
            {
                var total = new List<Property>();
                if (Head is Operartor)
                    total.AddRange(Operartor.GetPropertiesInside(Head as Operartor));
                else if (Head is Property)
                    total.Add(Head as Property);
                if (Expression is Operartor)
                    total.AddRange(Operartor.GetPropertiesInside(Expression as Operartor));
                else if (Expression is Property)
                    total.Add(Expression as Property);
                return total;
            }
        }

        public void Update(CalculatingEnvironment environment, Pack pack)
        {
            if (!string.IsNullOrWhiteSpace(RawHead))
                Head = FormulaElement.ParseElement(RawHead, environment, pack);
            if (!string.IsNullOrWhiteSpace(RawExpression))
                Expression = FormulaElement.ParseElement(RawExpression, environment, pack);
        }

        public Formula ExpressFrom(Property needs)
        {
            if (Expression is not Operartor)
                return null;
            if (!(Expression as Operartor).ToString().Contains(needs.ToString()))
                return null;
            FormulaElement nExp = Head;
            FormulaElement nHead = Expression as Operartor;
            while (nHead is Operartor)
            {
                var localHead = nHead as Operartor;
                var part = localHead.Arguments.FirstOrDefault(i => i.ToString().Contains(needs.ToString()));
                nExp = (nHead as Operartor).ExpressForm(part, nExp);
                nHead = part;
            }

            return new Formula { Expression = nExp, Head = nHead };
        }

        public override string ToString()
        {
            return ToView();
        }

        public string ToView()
        {
            var body = Expression.ToView();
            if (body[0] == '(')
                body = body.Substring(1, body.Length - 2);
            return $"{Head.ToView()} = {body}";
        }

        public string ToValue()
        {
            var body = Expression.ToValue();
            if (body[0] == '(')
                body = body.Substring(1, body.Length - 2);
            return $"{Head.ToView()} = {body}";
        }

        public string RawView => $"{RawHead} = {RawExpression}";
        public string View => ToView();

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}