namespace AutoMind
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, mind!");
            var testString = "abc[basdas;sadsaa;asdaw[uyrtyu;tuy56]];tests[];abab";
            var splited = Utils.SplitMultyMarker(testString);
        }
    }
}