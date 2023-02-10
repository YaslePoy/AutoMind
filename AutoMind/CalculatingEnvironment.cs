using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMind
{
    public class CalculatingEnvironment
    {
        public static readonly List<Opeartor> Operators = new List<Opeartor> { new Addition(), new Subtraction(), new Multiplication(), new Division() };
        public List<Property> Properties;
        public List<Constant> Constants;
        public List<Formula> Functions;
        public CalculatingEnvironment()
        {
            Properties = new List<Property>();
            Constants = new List<Constant>();
            Functions = new List<Formula>();
        }
        public void AddEnviromentPack(string import)
        {
            var file = File.ReadAllText(import);
            var doc = new MLLDocument(file);
            doc.Parce();
            if (doc.HasList("PR"))
                Properties.AddRange(doc["PR"].ParceList<Property>());
            if (doc.HasList("CN"))
                Constants.AddRange(doc["CN"].ParceList<Constant>());
            if (doc.HasList("EQ"))
                doc["EQ"].ParceList<Formula>().ForEach(f =>
                {
                    f.Update(this);
                    Functions.Add(f);
                });
        }
    }
}
