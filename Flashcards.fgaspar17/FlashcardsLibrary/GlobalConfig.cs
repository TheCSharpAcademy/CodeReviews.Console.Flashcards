namespace FlashcardsLibrary;

public static class GlobalConfig
{
    public static string? ConnectionString { get; set; }
    public static string? SetupConnectionString { get; set; }

    public static void InitializeConnectionString(string? connectionString)
    {
        ConnectionString = connectionString;
    }

    public static void InitializeSetupConnectionString(string? setupConnectionString)
    {
        SetupConnectionString = setupConnectionString;
    }
}