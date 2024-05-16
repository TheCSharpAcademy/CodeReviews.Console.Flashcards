namespace Flashcards.Services;

public class ConfigSettings
{
    // <!--' Connection Strings '-->
    public static string DatabaseMasterConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseMasterConnectionString"].ConnectionString;
    public static string DatabaseFlashcardsConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseFlashcardsConnectionString"].ConnectionString;

    // <!--' Database Constants | Database, Table, and View names '-->
    public static string DatabaseName => System.Configuration.ConfigurationManager.AppSettings["DatabaseName"]!;
    public static string TableNameStack => System.Configuration.ConfigurationManager.AppSettings["TableNameStack"]!;
    public static string TableNameFlashCards => System.Configuration.ConfigurationManager.AppSettings["TableNameFlashCards"]!;
    public static string TableNameStudySessions => System.Configuration.ConfigurationManager.AppSettings["TableNameStudySessions"]!;
    public static string ViewNameFlashCards => System.Configuration.ConfigurationManager.AppSettings["ViewNameFlashCards"]!;
    public static string ViewNameStudySessions => System.Configuration.ConfigurationManager.AppSettings["ViewNameStudySessions"]!;
    public static string ViewNameFlashCardsRenumbered => System.Configuration.ConfigurationManager.AppSettings["ViewNameFlashCardsRenumbered"]!;

    // <!--' Spectre Console Menu Constants '-->
    public static string MenuTitle => System.Configuration.ConfigurationManager.AppSettings["MenuTitle"]!;
    public static int PageSize => Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"]!);
}
