using System.Text;
using System.Text.Json;
using Flashcards.glaxxie.Controllers;
using Flashcards.glaxxie.Display;
using Flashcards.glaxxie.DTO;
using Flashcards.glaxxie.Prompts;
namespace Flashcards.glaxxie.DataSeeder;

internal class Seeder
{
    internal static void GenerateData()
    {
        Console.WriteLine("Main purpose of this setting is to seed data for quick demonstration of the app.");
        if (General.ConfirmationInput("Generate some stacks and cards for the app?"))
            GenerateCards();

        if (General.ConfirmationInput("Generate some sessions for the app?"))
            GenerateSessions();
    }   

    private static void GenerateCards(int topic = 7, int cards = 15)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Data", "seeder_data.json");
        var json = File.ReadAllText(path, Encoding.UTF8);
        var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json)!;

        var rand = new Random();
        var randomTopics = data.Keys.OrderBy(_ => rand.Next()).Take(topic);
        try
        {
            foreach (var stack in randomTopics)
            {

                int stackId = StackController.Insert(new StackCreation(stack));
                var selectedCards = data[stack].OrderBy(_ => rand.Next()).Take(cards);
                foreach (var c in selectedCards)
                {
                    CardCreation card = new(stackId, c.Key, c.Value);
                    CardController.Insert(card);
                }
            }
        } catch ( Exception)
        {
            Menu.ClearDisplay("Duplicate data. Press any key to continue");
            return;
        }
        Menu.ClearDisplay($"Finished seeding data: {topic} stacks, {cards} cards per stack.\nPress any key to go back to menu");
    }

    private static void GenerateSessions(int num = 500)
    {
        var now = DateTime.Now;
        var past = DateTime.Now.AddYears(-3);
        var range = (now - past).Days;
        var stacks = StackController.GetAllStacks();

        var rand = new Random();
        for (int i = 0; i < num; i++)
        {
            var nextDate = past.AddDays(rand.Next(range));
            var stackId = stacks[rand.Next(stacks.Count)].StackId;
            var cards = rand.Next(10, 15);
            var score = rand.Next(cards);
            SessionCreation ses = new(stackId, nextDate, cards, score);
            SessionController.Insert(ses);
        }
        Menu.ClearDisplay($"Finished seeding {num} sessions from {past.ToShortDateString()} to {now.ToShortDateString()}");
    }
}