namespace AutoMind
{
    public class Constant : Number
    {
        public override string DataType => "CONST";
        public string Name {  get; set; }
        public string View { get; set; }

        public override string ToString()
        {
            return $"CN.{View}";
        }
        public override string ToView()
        {
            return View;
        }

        public string NameView => Name.Replace('_', ' ');
    }

}
