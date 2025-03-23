namespace FlashCards
{
    internal interface IFlashCardRepository : IRepository<FlashCard>
    {
        IEnumerable<FlashCardDto> GetAllCardsInStack(CardStack stack);
    }
}
