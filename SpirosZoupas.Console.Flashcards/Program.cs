using Flashcards;
using Flashcards.DAL;

Repository repository = new Repository();
Controller controller = new Controller(repository);
UserInput userInput = new UserInput(controller);
userInput.GetUserInput();