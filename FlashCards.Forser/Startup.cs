using Microsoft.Extensions.Configuration;

public class Startup
{
    public Startup()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();
        AppSettings = config.GetSection("FlashcardDBContext").Get<AppSettings>();
    }

    public AppSettings AppSettings { get; private set; }
}

public class AppSettings
{
    public string ConnectionString { get; set; }
}