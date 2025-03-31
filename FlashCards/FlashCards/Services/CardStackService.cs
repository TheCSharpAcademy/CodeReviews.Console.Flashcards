using Microsoft.IdentityModel.Tokens;

namespace FlashCards
{
    /// <summary>
    /// Represents a service for managing CardStack entities.
    /// Implements ICardStackService
    /// </summary>
    internal class CardStackService : ICardStackService
    {
        /// <inheritdoc/>
        public ICardStackRepository CardStackRepository { get; set; }
        /// <inheritdoc/>
        public ICardStackServiceUi UserInterface { get; set; }

        /// <summary>
        /// Intializes new object of CardStackService class
        /// </summary>
        /// <param name="repository">A implementation of ICardStackRepository for database access</param>
        /// <param name="UI">A implementation of ICardStackRepository for user interaction</param>
        public CardStackService(ICardStackRepository repository, ICardStackServiceUi UI)
        {
            CardStackRepository = repository;
            UserInterface = UI;
        }
        /// <inheritdoc/>
        public List<CardStack> GetAllStacks() => CardStackRepository.GetAllRecords().ToList();

        /// <summary>
        /// Displays information message to user that no CardStack entities were retrieved from the database
        /// </summary>
        private void HandleEmptyStack()
        {
            Console.WriteLine("No Stacks found");
            UserInterface.PrintPressAnyKeyToContinue();
        }
        /// <inheritdoc/>
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
        /// <inheritdoc/>
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
        /// <inheritdoc/>
        public void HandleCreateNewStack()
        {
            string stackName = UserInterface.GetStringFromUser("Enter new stack name: ");

            bool wasActionSuccessful = CardStackRepository.Insert(new CardStack() { StackName = stackName });
            Console.WriteLine(wasActionSuccessful ? "Stack created successfully" : "Error occured, please contact admin");
            UserInterface.PrintPressAnyKeyToContinue();
        }
        /// <inheritdoc/>
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
        /// <inheritdoc/>
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
