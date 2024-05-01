namespace Flashcards.Services;

public class ConfigSettings
{
    // <!--' Connection Strings '-->
    public static string DbMasterConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["dbMasterConnectionString"].ConnectionString;
    public static string DbFlashcardsConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["dbFlashcardsConnectionString"].ConnectionString;

    // <!--' Database Constants | Database, Table, and View names '-->
    public static string DbName => System.Configuration.ConfigurationManager.AppSettings["DbName"]!;
    public static string TbStackName => System.Configuration.ConfigurationManager.AppSettings["TblStackName"]!;
    public static string TbFlashCardsName => System.Configuration.ConfigurationManager.AppSettings["TblFlashCardsName"]!;
    public static string TbStudySessionsName => System.Configuration.ConfigurationManager.AppSettings["TblStudySessionsName"]!;
    public static string VwFlashCardsName => System.Configuration.ConfigurationManager.AppSettings["VwFlashCardsName"]!;
    public static string VwStudySessionsName => System.Configuration.ConfigurationManager.AppSettings["VwStudySessionsName"]!;
    public static string VwFlashCardsRenumberedName => System.Configuration.ConfigurationManager.AppSettings["VwFlashCardsRenumberedName"]!;

    // <!--' Spectre Console Menu Constants '-->
    public static string MenuTitle => System.Configuration.ConfigurationManager.AppSettings["MenuTitle"]!;
    public static int PageSize => Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"]!);
}
