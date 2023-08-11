using Flashcards.MartinL_no.DAL;
using Flashcards.MartinL_no.Models;

var connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");
var stackRepo = new FlashcardStackRepository(connectionString);

var flashcards = new Stack<string>();
flashcards.Push("text");
var stack = new FlashcardStack("name", flashcards);

//var result = stackRepo.InsertStack(stack);
//var result = stackRepo.GetStacks();
//var result = stackRepo.DeleteStack(stack);
var result = stackRepo.DeleteFlashCard("text", stack.Name);
Console.WriteLine(result);