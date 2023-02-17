namespace AutoMind
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, mind!");

            var env = new CalculatingEnvironment();
            env.AddEnviromentPack("emf");
            var field = env.LinkedEnviroment();
            var way = new List<object>();
            var hasWay = field.HasWayBeet(env.Properties[1], env.Properties[16], new List<LinkedElement>(), ref way);
        }
    }
}