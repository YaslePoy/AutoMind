﻿namespace AutoMind
{
    public class CalculatingEnvironment
    {
        public IPackSourceProvider PackSourceProvider;
        public static readonly List<Operartor> Operators = new List<Operartor>
        {
            new Addition(), new Subtraction(), new Multiplication(), new Division(), new Negative(), new Square(),
            new Root()
        };

        public List<Property> Properties { get; set; }
        public List<Constant> Constants { get; set; }
        public List<Formula> Functions { get; set; }
        public List<Pack> ImportPacks { get; set; }
        public Property GetProperty(string view, Pack box)
        {
            var splited = view.Split(new char[] { '.', ':' },
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (splited.Length == 3)
            {
                return Properties.FirstOrDefault(i => i.Origin.Short == splited[0] && i.View == splited[2]);
            }
            else
            {
                var temp = Properties.FirstOrDefault(i => i.Origin == box && i.View == splited[1]);
                if (temp is not null)
                    return temp;
                else
                    return Properties.FirstOrDefault(i => i.View == splited[1]);
            }
        }

        public Constant GetConstant(string view, Pack box)
        {
            var splited = view.Split(new char[] { '.', ':' },
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (splited.Length == 3)
            {
                return Constants.FirstOrDefault(i => i.Origin.Short == splited[0] && i.View == splited[2]);
            }
            else
            {
                var temp = Constants.FirstOrDefault(i => i.Origin == box && i.View == splited[1]);
                if (temp is not null)
                    return temp;
                else
                    return Constants.FirstOrDefault(i => i.View == splited[1]);
            }
        }

        public CalculatingEnvironment(IPackSourceProvider packSourceProvider)
        {
            PackSourceProvider = packSourceProvider;
            Properties = new List<Property>();
            Constants = new List<Constant>();
            Functions = new List<Formula>();
            ImportPacks = new List<Pack>();
        }

        public void AddRawEnvironmentPack(string data)
        {
            var doc = new MLLDocument(data);
            doc.Parce();
            var addPack = doc["INFO"][0].Parce<Pack>();
            if (ImportPacks.Any(i => i.Identifier == addPack.Identifier))
                return;
            ImportPacks.Add(addPack);
            if (doc.HasList("IM"))
            {
                doc["IM"].ForEach(x => AddEnvironmentPack(x.Data["import"]));
            }
            if (doc.HasList("PR"))
            {
                var rawList = doc["PR"].ParceList<Property>();
                addPack.Properties = rawList;
                rawList.ForEach(i => i.Origin = addPack);
                Properties.AddRange(rawList);
            }

            if (doc.HasList("CN"))
            {
                var rawList = doc["CN"].ParceList<Constant>();
                addPack.Constants = rawList;
                rawList.ForEach(i => i.Origin = addPack);
                Constants.AddRange(rawList);
            }

            if (doc.HasList("EQ"))
            {
                var rawList = doc["EQ"].ParceList<Formula>();
                addPack.Formulas = rawList;
                rawList.ForEach(f =>
                {
                    f.Update(this, addPack);
                    Functions.Add(f);
                    f.Origin = addPack;
                });
            }

        }

        public void AddEnvironmentPack(string import)
        {
            var file = PackSourceProvider.GetPackSourceText(import);
            AddRawEnvironmentPack(file);
        }


        public MultilinkedField LinkedEnvironment()
        {
            var mlf = new MultilinkedField();
            foreach (var f in Functions)
            {
                foreach (var p in f.TotalProperties)
                {
                    mlf.AddLink(f, p);
                }
            }

            return mlf;
        }
    }
}