using Microsoft.Extensions.Configuration;
using FlashcardApp.Console.IoC;
using FlashcardApp.Console.Menus;
using FlashcardApp.Console.MessageLoggers;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    public static IConfiguration Configuration { get; set; }

    public async static Task Main(string[] args)
    {
        try
        {
            LoadAppSettingConfiguration();
            var serviceProvider = LoadIocConfiguration();
            while (true)
            {
                await LoadDisplayMenu(serviceProvider);
            }
        }
        catch (Exception ex)
        {
            MessageLogger.DisplayErrorMessage(ex.Message);
        }
    }

    private static async Task LoadDisplayMenu(IServiceProvider serviceProvider)
    {
        var mainMenu = serviceProvider.GetRequiredService<MainMenu>();
        await mainMenu.DisplayMenu();
    }

    private static IServiceProvider LoadIocConfiguration()
    {
        string connectionString = GetConnectionString();
        var services = new ServiceCollection();
        DependencyInjectionConfig.ConfigureServices(services, connectionString);
        return services.BuildServiceProvider();
    }

    private static void LoadAppSettingConfiguration(){
        var builder = new ConfigurationBuilder()
            .SetBasePath(GetCurrentPath())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        Configuration = builder.Build();
    }

    private static string GetCurrentPath()
    {
        return Environment.CurrentDirectory.Replace("bin/Debug/net8.0", "");
    }

    private static string GetConnectionString(){
        return Configuration.GetConnectionString("DefaultConnection");
    }
}
