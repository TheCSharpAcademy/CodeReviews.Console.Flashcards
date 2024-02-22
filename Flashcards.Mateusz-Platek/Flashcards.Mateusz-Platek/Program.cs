namespace Flashcards.Mateusz_Platek;

public static class Program
{
    public static void Main()
    {
        DatabaseHelper.CreateDatabase();
        DatabaseHelper.CreateTables();
        if (Menu.GetDummyData() == "Yes")
        {
            DatabaseHelper.InsertData();
        }
        Menu.Run();
    }
}