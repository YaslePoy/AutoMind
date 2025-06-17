using System.ComponentModel.DataAnnotations.Schema;

namespace PhysLab.DB;

public class Solution : DbNamed
{
    [ForeignKey("User")]
    public int UserId { get; set; }

    public virtual User User { get; set; }
    public string InnerData { get; set; } = "";
    public string Description { get; set; }
}

public class ConnectedPacks : DbEntity
{
    [ForeignKey("Solution")]
    public int SolutionId { get; set; }

    public virtual Solution Solution { get; set; }

    [ForeignKey("EnviromentPack")]
    public int EnvironmentPackId { get; set; }

    public virtual EnvironmentPack EnvironmentPack { get; set; }
}

public class SolutionCalculation : DbEntity
{
    [ForeignKey("Solution")]
    public int SolutionId { get; set; }

    public virtual Solution Solution { get; set; }
    public string Formula { get; set; }
    public string Head { get; set; }
}