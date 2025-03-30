using Microsoft.IdentityModel.Tokens;

namespace FlashCards
{
    internal class CardStackService : ICardStackService
    {
        public ICardStackRepository CardStackRepository { get; set; }
        public ICardStackServiceUi UserInterface { get; set; }

        public CardStackService(ICardStackRepository repository, ICardStackServiceUi UI)
        {
            CardStackRepository = repository;
            UserInterface = UI;
        }
        public List<CardStack> GetAllStacks() => CardStackRepository.GetAllRecords().ToList();
        private void HandleEmptyStack()
        {
            Console.WriteLine("No Stacks found");
            UserInterface.PrintPressAnyKeyToContinue();
        }
        public bool PrepareRepository(List<CardStack> defaultData)
        {
            try
            {
                if (!CardStackRepository.DoesTableExist())
                {
                    CardStackRepository.CreateTable();
                    CardStackRepository.AutoFill(defaultData);
                }
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Error while preparing the repository");
                return false;
            }

        }
        public void HandleViewAllStacks()
        {
            List<CardStack> stacks = GetAllStacks();

            if (stacks.IsNullOrEmpty())
            {
                HandleEmptyStack();
                return;
            }

            UserInterface.PrintStacks(stacks);
            UserInterface.PrintPressAnyKeyToContinue();

        }
        public void HandleCreateNewStack()
        {
            string stackName = UserInterface.GetStringFromUser("Enter new stack name: ");

            bool wasActionSuccessful = CardStackRepository.Insert(new CardStack() { StackName = stackName });
            Console.WriteLine(wasActionSuccessful ? "Stack created successfully" : "Error occured, please contact admin");
            UserInterface.PrintPressAnyKeyToContinue();
        }
        public void HandleRenameStack()
        {
            List<CardStack> stacks = GetAllStacks();
            if (stacks.IsNullOrEmpty())
            {
                HandleEmptyStack();
                return;
            }

            CardStack stack = UserInterface.StackSelection(stacks);

            stack.StackName = UserInterface.GetStringFromUser("Please enter new name: ");

            bool wasActionSuccessful = CardStackRepository.Update(stack);
            Console.WriteLine(wasActionSuccessful ? "Stack renamed successfully" : "Error occured, please contact admin");
            UserInterface.PrintPressAnyKeyToContinue();
        }
        public void HandleDeleteStack()
        {
            List<CardStack> stacks = GetAllStacks();

            if (stacks.IsNullOrEmpty())
            {
                HandleEmptyStack();
                return;
            }

            CardStack stack = UserInterface.StackSelection(stacks);

            bool wasActionSuccessful = CardStackRepository.Delete(stack);
            Console.WriteLine(wasActionSuccessful ? "Stack deleted successfully" : "Error occured, please contact admin");
            UserInterface.PrintPressAnyKeyToContinue();
        }
    }
}
