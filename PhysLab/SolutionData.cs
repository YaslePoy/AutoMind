using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMind;
using PhysLab.DB;

namespace PhysLab;

public class SolutionData
{
    public List<PropertyView> PropertyViews { get; set; } = new();
    [JsonIgnore]
    public List<Property> Properties { get; set; } = new();
    public List<string> FormulaViews { get; set; } = new();
    [JsonIgnore]
    public List<Formula> Formulas { get; set; } = new();
    public List<PropertyCommutation> Commutations { get; set; } = new();

    [JsonIgnore]
    public Solution Solution { get; set; }
    public async Task SaveAsync()
    {
        PropertyViews.Clear();
        PropertyViews.AddRange(Properties.Select(i => i.ToPropertyView()));
        Solution.InnerData = JsonSerializer.Serialize(this);
        await PhysContext.Instance.SaveChangesAsync();
    }
}

public class PropertyView
{
    public string Identifier { get; set; }
    public double Value { get; set; }
}

public class PropertyCommutation
{
    public int PropertyIndex { get; set; }
    public int FormulasIndex { get; set; }
    public int FormulaPropertyIndex { get; set; }
}