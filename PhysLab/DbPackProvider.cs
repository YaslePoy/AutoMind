using AutoMind;
using PhysLab.DB;

namespace PhysLab;

public class DbPackProvider : IPackSourceProvider
{
    public string GetPackSourceText(string name)
    {
        return PhysContext.Instance.EnvironmentPacks.FirstOrDefault(en => en.Identifier == name).Data;
    }
}