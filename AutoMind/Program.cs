namespace AutoMind
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, mind!");
            var env = new CalculatingEnvironment();
            env.AddEnviromentPack("total");
            var view = env.Properties[23].UnitsView;
            int h =  env.Properties[0].GetHashCode();

        }
    }
}