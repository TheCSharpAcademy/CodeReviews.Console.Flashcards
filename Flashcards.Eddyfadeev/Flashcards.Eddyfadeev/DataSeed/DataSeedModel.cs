using Flashcards.Eddyfadeev.Models.Entity;
using Newtonsoft.Json;

namespace Flashcards.Eddyfadeev.DataSeed;

/// <summary>
/// The SeedDataModel class contains two properties: Stacks and Flashcards.
/// These properties are used to store the seed data that will be used to populate the database. 
/// The Stacks property is a list of Stack objects, which represent a collection of flashcards.
/// The Flashcards property is a list of Flashcard objects, which represent individual flashcards. 
/// The SeedDataModel class is internal.
/// </summary>
public class DataSeedModel
{
    /// <summary>
    /// Represents a stack of flashcards.
    /// </summary>
    [JsonProperty]
    internal List<Stack> Stacks { get; set; } = new();

    /// <summary>
    /// Represents a flashcard entity.
    /// </summary>
    [JsonProperty]
    internal List<Flashcard> Flashcards { get; set; } = new();
}