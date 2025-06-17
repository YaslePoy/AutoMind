namespace AutoMind
{
    public class Property : Constant
    {
        public override string DataType => "PROP";
        public string UnitsFull { get; set; }
        public string UnitsShort { get; set; }
        public string UnitsView => UnitsFull.Replace('_', ' ');

        public Property Clone()
        {
            return MemberwiseClone() as Property;
        }
    }
}
