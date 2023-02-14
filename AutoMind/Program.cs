namespace AutoMind
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, mind!");
            var env = new CalculatingEnvironment();
            env.AddEnviromentPack("formulaPacks/emf.ep");
            var props = env.Functions[0].ExpressFrom(env.Properties.FirstOrDefault(i => i.View == "B"));

        }
    }
}