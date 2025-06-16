namespace PhysLab.DB;

public class User : DbNamed
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}