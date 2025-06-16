using System.ComponentModel.DataAnnotations.Schema;

namespace PhysLab.DB;

public class PackDependency : DbEntity
{
    [ForeignKey("Pack")]
    public int PackId { get; set; }
    public virtual EnvironmentPack Pack { get; set; }
    [ForeignKey("Dependency")]
    public int DependencyId { get; set; }
    public virtual EnvironmentPack Dependency { get; set; }
}