using System.Text;

namespace AutoMind
{
    internal class PhysicalTask
    {
        MultilinkedField CalcField;
        CalculatingEnvironment currentEnv;
        public PhysicalTask(CalculatingEnvironment environment)
        {
            this.currentEnv = environment;
            CalcField = currentEnv.LinkedEnvironment();
        }

        public string Solve(List<Property> given, Property needs)
        {
            var solve = new StringBuilder();
            var startFormula = currentEnv.Functions.FirstOrDefault(i => i.TotalProperties.Count(j => given.Contains(j)) == i.TotalProperties.Count() - 1);
            if (startFormula == null)
                return solve.ToString();
            if (startFormula.TotalProperties.Contains(needs))
            {
                Formula finalFormula = startFormula;
                if(startFormula.Head != needs)
                {
                    solve.AppendLine(startFormula.ToView() + " =>");
                    finalFormula = startFormula.ExpressFrom(needs);
                    (finalFormula.Head as Property).Value = finalFormula.Expression.GetValue();
                }
                
                solve.AppendLine(finalFormula.ToView() + " =>");
                solve.AppendLine(finalFormula.ToValue() + " = " + finalFormula.Head.ToValue());
                return solve.ToString();
            }
            if (startFormula.TotalProperties.Any(i => needs == i))
            {
                Console.WriteLine($"{startFormula.ToView()} => {startFormula.ExpressFrom(needs).ToView()}");
            }
            else
            {
                List<object> way = new List<object>();
                if (CalcField.HasWayBeet(startFormula, needs, new List<LinkedElement>(), ref way))
                {
                    var chain = way.Select(i => i as FormulaElement).ToList();
                    for (int i = 1; i < chain.Count; i += 2)
                    {

                    }
                }
            }
            
            return null;
            
        }
    }
}
