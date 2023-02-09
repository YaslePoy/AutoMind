using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMind
{
    public class Formula
    {
        public string RawHead;
        public string RawExpression;
        public FormulaElement Expression;
        public FormulaElement Head;
        public List<Property> TotalProperties
        {
            get
            {
                var total = new List<Property>();
                if (Head.DataType == "OPERATOR")
                    total.AddRange(Opeartor.GetPropertiesInside(Head as Opeartor));
                else if (Head.DataType == "PROP")
                    total.Add(Head as Property);
                if (Expression.DataType == "OPERATOR")
                    total.AddRange(Opeartor.GetPropertiesInside(Expression as Opeartor));
                else if (Expression.DataType == "PROP")
                    total.Add(Expression as Property);
                return total;
            }
        }
        public void Update(CalculatingEnvironment environment)
        {
            if(!string.IsNullOrWhiteSpace(RawHead)) 
                Head = FormulaElement.ParceElement(RawHead, environment);
            if(!string.IsNullOrWhiteSpace(RawExpression))
                Expression = FormulaElement.ParceElement(RawExpression, environment);
        }
    }
}
