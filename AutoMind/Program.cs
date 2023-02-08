namespace AutoMind
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, mind!");
            //CalculatingEnvironment ce = new CalculatingEnvironment();
            //ce.AddEnviromentPack(@"formulaPacks\testPack.ep");
            var file = File.ReadAllText(@"formulaPacks\testPack.ep");
            var doc = new MLLDocument(file);
            doc.Parce();
            var props = doc["CN"].ParceList<Constant>();
            Console.WriteLine(props[0]);
        }
    }
}