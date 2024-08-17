using Flashcards.Eddyfadeev.Interfaces.Database;
using Flashcards.Eddyfadeev.Services;
using Newtonsoft.Json;

namespace Flashcards.Eddyfadeev.DataSeed;

/// <summary>
/// This class provides methods for seeding data into the database.
/// </summary>
internal static class DataSeed
{
    /// <summary>
    /// Processes the request by reading data from a JSON file, deserializing it, and inserting the data into the database.
    /// </summary>
    /// <param name="databaseManager">The database manager.</param>
    /// <param name="databaseInitializer">The database initializer used to initialize the database.</param>
    internal static void ProcessRequest(IDatabaseManager databaseManager, IDatabaseInitializer databaseInitializer)
    {
        var jsonString = File.ReadAllText(@"DataSeed\DataSeed.json");

        DataSeedModel seedData = DeserializeJson(jsonString);
        
        if (seedData is null)
        {
            Console.WriteLine("Seed data is null.");
            GeneralHelperService.ShowContinueMessage();
        }
        
        if (seedData.Flashcards.Count == 0 || seedData.Stacks.Count == 0)
        {
            Console.WriteLine("Deserialization failed.");
            GeneralHelperService.ShowContinueMessage();
            return;
        }
        
        databaseManager.DropForeignKeyConstraints();
        databaseManager.DeleteTables();
        databaseInitializer.Initialize();
        var result = databaseManager.BulkInsertRecords(seedData.Stacks, seedData.Flashcards);

        Console.WriteLine(result ? "Data seed was successful." : "Data seed failed.");
        GeneralHelperService.ShowContinueMessage();
    }

    private static DataSeedModel? DeserializeJson(string json)
    {
        try
        {
            var dataSeed = JsonConvert.DeserializeObject<DataSeedModel>(json);

            if (dataSeed?.Flashcards.Count == 0 || dataSeed?.Stacks.Count == 0)
            {
                Console.WriteLine("Deserialization couldn't fill the list.");
                GeneralHelperService.ShowContinueMessage();
            }

            return dataSeed;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Deserialization failed: {ex.Message}");
            GeneralHelperService.ShowContinueMessage();
            return null;
        }
    }
}