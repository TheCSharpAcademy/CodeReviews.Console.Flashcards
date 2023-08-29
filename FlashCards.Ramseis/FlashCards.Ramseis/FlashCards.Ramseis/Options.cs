using System.Configuration;
using System.Collections.Specialized;

namespace FlashCards.Ramseis
{
    internal class Options
    {
        static public string databaseName { get; set; } = "Flashcards";
        static public string connectionString = @"Data Source = (localdb)\" + databaseName;

        public static void WriteSetting(string key, string value)
        {
            try
            {
                Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing settings.");
            }
        }
        public static string ReadSetting(string key)
        {
            try
            {
                NameValueCollection settings = ConfigurationManager.AppSettings;
                return settings[key] ?? "";
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine($"Error reading setting {key}.");
            }
            return "";
        }
        public void ReadAll()
        {

        }
    }
}
