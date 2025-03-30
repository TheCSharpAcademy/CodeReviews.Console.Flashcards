
namespace FlashCards
{
    internal interface IFlashCardRepository
    {
        string ConnectionString { get; }

        void AutoFill(List<CardStack> stacks, List<FlashCard> flashCards);
        bool CreateTable();
        bool Delete(FlashCard entity);
        bool DoesTableExist();
        IEnumerable<FlashCard>? GetAllRecords();
        IEnumerable<FlashCardDto>? GetAllRecordsFromStack(CardStack stack);
        IEnumerable<FlashCardDto>? GetXRecordsFromStack(CardStack stack, int count);
        bool Insert(FlashCard entity);
        bool Update(FlashCard entity);
    }
}