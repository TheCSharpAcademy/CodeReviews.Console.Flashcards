using FlashCards.Data;
using FlashCards.View;
using Microsoft.EntityFrameworkCore;

namespace FlashCards
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=Phonebook;Trusted_Connection=True;Encrypt=false;");

            using var context = new DataContext(optionsBuilder.Options);
            var stackService = new Services.StackService(context);
            var flashcardService = new Services.FlashcardService(context);
            var studySessionService = new Services.StudySessionService(context, flashcardService, stackService);
            var userInterface = new UserInterface(stackService, flashcardService, studySessionService);

            userInterface.MainMenu();
        }
    }
}