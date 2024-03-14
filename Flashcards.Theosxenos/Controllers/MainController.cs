namespace Flashcards.Controllers;

public class MainController
{
    public void ShowMainMenu()
    {
        var deckController = new StackController();
        var flashcardController = new FlashcardController();
        var practiceController = new PracticeController();
        var view = new BaseView();

        var run = true;
        while (run)
        {
            var menuOptions = new Dictionary<string, Action>
            {
                ["Create Stack"] = () => deckController.CreateStack(),
                ["Manage Stack"] = () => deckController.ManageStack(),
                ["Create Flashcard"] = () => flashcardController.CreateFlashcard(),
                ["Update Flashcard"] = () => flashcardController.UpdateFlashcard(),
                ["List Flashcards"] = () => flashcardController.ListFlashcards(),
                ["Start Practice"] = () => practiceController.StartSession(),
                ["Practice Log"] = () => practiceController.ShowPracticeLog(),
                ["Exit"] = () => run = false
            };

            try
            {
                var choice = view.ShowMenu(menuOptions.Keys.ToArray());
                menuOptions[choice]();
            }
            catch (NotFoundException e)
            {
                view.ShowError(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}