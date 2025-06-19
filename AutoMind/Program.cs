namespace AutoMind
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, mind!");
            var env = new CalculatingEnvironment(new FilePackSourceProvider());
            env.AddEnvironmentPack("emf");
            var task = new PhysicalTask(env);
            Console.WriteLine(env.Functions.First().ToView());
            List<Property> now = new List<Property>();
            Property need = env.Properties.Find(i => i.View == "L");
            now.Add(env.Properties.Find(i => i.View == "W"));
            now.Add(env.Properties.Find(i => i.View == "I"));
            now[0].Value = 0.32;
            now[1].Value = 7.4;
            Console.WriteLine(task.Solve(now, need));
            //var field = env.LinkedEnviroment();
            //var way = new List<object>();
            //var hasWay = field.HasWayBeet(env.Properties[1], env.Properties[16], new List<LinkedElement>(), ref way);
        }
    }
}