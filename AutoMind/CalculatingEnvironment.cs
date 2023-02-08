using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMind
{
    public class CalculatingEnvironment
    {
        public List<Type> Operators;
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
            var file = File.ReadAllLines(import).ToList();
            string propNick = String.Empty;
            string cosntNick = String.Empty;
            var propLs = new List<string>();
            var constLs = new List<string>();
            for (int i = 0; i < file.Count; i++)
            {
                var line = file[i];
                if (line.Contains("define"))
                {
                    var eq = Utils.GetDefinition(line);
                    if(eq.name == Property.Type)
                    {
                        propNick = eq.nick;
                        //AddProperties();
                    }
                }
            }
        }
        public void AddProperties(List<string> properties)
        {
            
        }
    }
}
