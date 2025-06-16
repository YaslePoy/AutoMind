using Microsoft.EntityFrameworkCore;

namespace PhysLab.DB;

public class PhysContext :DbContext
{
    public static PhysContext Instance = new PhysContext();
    public PhysContext() : base()
    {
    }

    public PhysContext(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Data Source=tcp:micialware.ru,1433\\\\\\\\SQLEXPRESS;Initial Catalog=phys_db;User ID=remote;Password=pass;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    public DbSet<User> Users { get; set; }
    public DbSet<Solution> Solutions { get; set; }
    public DbSet<EnvironmentPack> EnvironmentPacks { get; set; }
    public DbSet<ConnectedPacks> ConnectedPacks { get; set; }
    public DbSet<SolutionCalculation> SolutionCalculations { get; set; }
    
    public static User User = PhysContext.Instance.Users.FirstOrDefault();
}