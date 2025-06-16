namespace AutoMind;

public interface IPackSourceProvider
{
    string GetPackSourceText(string name);
}

public class FilePackSourceProvider : IPackSourceProvider
{
    public string GetPackSourceText(string name)
    {
        return File.ReadAllText("formulaPacks/" + name + ".ep");
    }
}