namespace FlashCards
{
    internal class CardStackRepositoryService
    {
        public CardStackRepository CardStackRepository { get; set; }
        public UserInterface UserInterface { get; set; }

        public CardStackRepositoryService(CardStackRepository repository, UserInterface UI)
        {
            CardStackRepository = repository;
            UserInterface = UI;
        }
        public List<CardStack> GetAllStacks()
        {
            return CardStackRepository.GetAllRecords().ToList();
        }

        public void PrepareRepository(List<CardStack> defaultData)
        {

            if (!CardStackRepository.DoesTableExist())
            {
                CardStackRepository.CreateTable();
                CardStackRepository.AutoFill(defaultData);
            }
        }
        public void HandleViewAllStacks()
        {
            List<CardStack> stacks = CardStackRepository.GetAllRecords().ToList();
            UserInterface.PrintStacks(stacks);
            UserInterface.PrintPressAnyKeyToContinue();

        }
        public void HandleCreateNewStack()
        {
            string stackName = UserInterface.GetNewText("Enter new stack name: ");

            bool wasActionSuccessful = CardStackRepository.Insert(new CardStack() { StackName = stackName });
            Console.WriteLine(wasActionSuccessful ? "Stack created successfully" : "Error occured, please contact admin");
            UserInterface.PrintPressAnyKeyToContinue();
        }
        public void HandleRenameStack()
        {
            List<CardStack> stacks = CardStackRepository.GetAllRecords().ToList();
            CardStack stack = UserInterface.StackSelection(stacks);

            stack.StackName = UserInterface.GetNewText("Please enter new name: ");

            bool wasActionSuccessful = CardStackRepository.Insert(stack);
            Console.WriteLine(wasActionSuccessful ? "Stack renamed successfully" : "Error occured, please contact admin");
            UserInterface.PrintPressAnyKeyToContinue();
        }
        public void HandleDeleteStack()
        {
            List<CardStack> stacks = CardStackRepository.GetAllRecords().ToList();
            CardStack stack = UserInterface.StackSelection(stacks);

            bool wasActionSuccessful = CardStackRepository.Delete(stack);
            Console.WriteLine(wasActionSuccessful ? "Stack deleted successfully" : "Error occured, please contact admin");
            UserInterface.PrintPressAnyKeyToContinue();
        }
    }
}
