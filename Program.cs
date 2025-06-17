using Database;

public class Program
{
    public static void Main(string[] args)
    {
        // Load Env
        DotNetEnv.Env.Load();
        var db = new DbContext();
        db.ConnectionStatus();
        var migration = new Migaration();
        migration.Up();
    }
}
