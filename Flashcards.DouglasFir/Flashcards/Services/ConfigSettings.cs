namespace Flashcards.Services;

public class ConfigSettings
{
    // <!--' Connection Strings '-->
    public static string dbMasterConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["dbMasterConnectionString"].ConnectionString;
    public static string dbFlashcardsConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["dbFlashcardsConnectionString"].ConnectionString;

    // <!--' Database Constants | Database, Table, and View names '-->
    public static string dbName => System.Configuration.ConfigurationManager.AppSettings["DbName"]!;
    public static string tbStackName => System.Configuration.ConfigurationManager.AppSettings["TblStackName"]!;
    public static string tbFlashCardsName => System.Configuration.ConfigurationManager.AppSettings["TblFlashCardsName"]!;
    public static string tbStudySessionsName => System.Configuration.ConfigurationManager.AppSettings["TblStudySessionsName"]!;
    public static string vwFlashCardsName => System.Configuration.ConfigurationManager.AppSettings["VwFlashCardsName"]!;
    public static string vwStudySessionsName => System.Configuration.ConfigurationManager.AppSettings["VwStudySessionsName"]!;
    public static string vwFlashCardsRenumberedName => System.Configuration.ConfigurationManager.AppSettings["VwFlashCardsRenumberedName"]!;

    // <!--' Spectre Console Menu Constants '-->
    public static string menuTitle => System.Configuration.ConfigurationManager.AppSettings["MenuTitle"]!;
    public static int pageSize => Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"]!);
}
