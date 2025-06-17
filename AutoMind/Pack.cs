namespace AutoMind
{
    public class Pack
    {
        public string Identifier { get; set; }
        public string Name { get; set; } = "NN";
        public string Description { get; set; }
        public string Short { get; set; }

        public List<Property> Properties { get; set; } = new();
        
        public List<Constant> Constants { get; set; } = new();
        public List<Formula> Formulas { get; set; } = new();
        public override string ToString()
        {
            return Name.Replace("_", " ");
        }
        public static bool operator ==(Pack p1, Pack p2)
        {
            return p1.Name + p1.Description + p1.Short == p2.Name + p2.Description + p2.Short;
        }

        public static bool operator !=(Pack p1, Pack p2)
        {
            return p1.Name + p1.Description + p1.Short != p2.Name + p2.Description + p2.Short;
        }
    }
}