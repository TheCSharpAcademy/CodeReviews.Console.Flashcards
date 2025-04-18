using Flashcards.DAL;

namespace Flashcards.UserInput
{
    public class BaseMenu
    {
        protected readonly Controller _controller;
        protected readonly Validation _validation;

        public BaseMenu(Controller controller, Validation validation)
        {
            _controller = controller;
            _validation = validation;
        }
    }
}
