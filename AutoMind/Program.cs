namespace AutoMind
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, mind!");
            var env = new CalculatingEnvironment();
            env.AddEnviromentPack("formulaPacks/testPack.ep");
            var props = env.Functions[0].TotalProperties;
            var f = env.Functions[0].Expression.ToString();
        }
    }
}