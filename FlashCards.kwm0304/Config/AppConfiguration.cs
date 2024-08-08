using Microsoft.Extensions.Configuration;

namespace FlashCards.kwm0304.Config;

public static class AppConfiguration
{
  public static IConfigurationRoot Configuration { get; private set; }
  static AppConfiguration()
  {
    Configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
  }
  public static string GetConnectionString(string name)
  {
    return Configuration.GetConnectionString(name) ?? "Configuration is not being acknowledged";
  }
}