using System.Runtime.CompilerServices;
using Spectre.Console;
using Microsoft.Extensions.Configuration;

namespace FlashcardsLibrary.Controllers;

internal static class Utilities
{
    private static readonly string? connectionString;
    internal static readonly string StackTableName = "Stack";
    internal static readonly string FlashcardTableName = "Flashcard";
    internal static bool EmptyStack;
    internal static string? currentStack {get; set;} 
    internal static readonly DatabaseManager databaseManager;

    static Utilities()
    {
        connectionString = new ConfigurationBuilder()
                               .AddJsonFile("appsettings.json", false, false)
                               .Build()["ConnectionString"] ?? "n/a";

        EmptyStack = DatabaseManager.StaticStackExists(connectionString) == 0 ? true : false;
        databaseManager = new();
    }
    internal static T GetSelection<T>(T[] enumerationValues,
                                      string title,
                                      Func<T, string>? alternateNames = null) 
                                      where T: notnull
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<T>()
            .Title(title)
            .PageSize(15)
            .AddChoices(enumerationValues)
            .UseConverter(item => alternateNames != null 
                          ? alternateNames(item) : item.ToString() 
                          ?? string.Empty)
        );     
    }

    internal static void PressToContinue()
    {
        System.Console.WriteLine("\nPress Any key To Continue...");
        System.Console.ReadKey();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string? GetDatabaseConnectionString()
    {
        return connectionString;
    }

    internal static string GetStringInput(string message="> ", string errorMessage="Invalid Input\n> ", Func<string, bool>? Condition = null)
    {
        System.Console.Write(message);
        string? input = System.Console.ReadLine();
        
        while(string.IsNullOrEmpty(input) || (Condition != null && !Condition(input)))
        {
            System.Console.Write(errorMessage);
            input = System.Console.ReadLine();
        }
        
        return input;
    }

    internal static int GetIntegerInput(string message="> ", string errorMessage="Invalid Input\n> ", int lowerRange = int.MinValue, int maxRange = int.MaxValue, Func<int, bool>? Condition = null)
    {
        int result;
        System.Console.Write(message);
        
        while
        (
            !int.TryParse(System.Console.ReadLine(), out result) 
            || (result < lowerRange || result > maxRange)
            || (Condition != null && !Condition(result)) 
        )
            System.Console.Write(errorMessage);

        return result;
    }
}