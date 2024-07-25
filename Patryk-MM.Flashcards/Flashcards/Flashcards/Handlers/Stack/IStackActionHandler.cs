namespace Flashcards.Handlers.Stack;
public interface IStackActionHandler {
    Task<bool> HandleAsync(Models.Stack stack);
}


