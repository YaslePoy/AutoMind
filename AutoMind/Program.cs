namespace AutoMind
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, mind!");
            CalculatingEnvironment ce = new CalculatingEnvironment();
            ce.AddEnviromentPack("formelsPacks/testPack.ep");

        }
    }
}