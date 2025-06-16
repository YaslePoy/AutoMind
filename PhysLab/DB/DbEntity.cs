using System.ComponentModel.DataAnnotations;

namespace PhysLab.DB;

public abstract class DbEntity
{
    [Key]
    public int Id { get; set; }
}

public abstract class DbNamed : DbEntity
{
    public string Name { get; set; }
}