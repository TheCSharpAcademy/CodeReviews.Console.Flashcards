using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

public class Program
{
    public static IConfiguration Configuration { get; set; }

    public static void Main(string[] args)
    {
        LoadAppSettingConfiguration();
        string connectionString = GetConnectionString();
        Console.WriteLine($"Connection String: {connectionString}");
    }
    private static void LoadAppSettingConfiguration(){
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        Configuration = builder.Build();
    }
    private static string GetConnectionString(){
        return Configuration.GetConnectionString("DefaultConnection");
    }
}