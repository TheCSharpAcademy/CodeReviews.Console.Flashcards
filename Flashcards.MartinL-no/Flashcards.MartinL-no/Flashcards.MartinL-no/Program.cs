using Flashcards.MartinL_no.Controllers;
using Flashcards.MartinL_no.DAL;
using Flashcards.MartinL_no.Models;

var connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");
var stackRepo = new FlashcardStackRepository(connectionString);
var controller = new FlashcardController(stackRepo);

var flashcards = new List<Flashcard>();
var flashcard = new Flashcard() { Id = 2, Original = "dolce vita", Translation = "the good life", StackId = 1 };
//flashcards.Add(flashcard);
//var stack = new FlashcardStack("italian", flashcards);

//var r = stackRepo.InsertStack(stack);
//var r = stackRepo.GetStacks();
//var r = stackRepo.GetStackByName("italian");
//var r = stackRepo.InsertFlashcard(flashcard);
//var r = stackRepo.UpdateFlashcard(flashcard);
//var r = stackRepo.DeleteStack(new FlashcardStack("spanish", new List<Flashcard>()));
//var r = stackRepo.DeleteFlashCard(new Flashcard(90, "gatto", "cat", "italian"));

var r = controller.GetStacks();
//var r = controller.GetStackByName("italian");
//var r = controller.CreateStack("spanish");
//var r = controller.CreateFlashcard("gatto", "cat", "italian");

Console.WriteLine(r);