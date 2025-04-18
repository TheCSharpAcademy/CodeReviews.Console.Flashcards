using Flashcards;
using Flashcards.DAL;
using Flashcards.UserInput;

Repository repository = new Repository();
Controller controller = new Controller(repository);
Validation validation = new Validation(controller);
UserInput userInput = new UserInput(controller, validation);
userInput.GetMainMenu();