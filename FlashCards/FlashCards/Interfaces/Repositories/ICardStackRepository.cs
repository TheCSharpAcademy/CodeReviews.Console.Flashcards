
namespace FlashCards
{
    internal interface ICardStackRepository
    {
        string ConnectionString { get; }

        void AutoFill(List<CardStack> defaultData);
        bool CreateTable();
        bool Delete(CardStack entity);
        bool DoesTableExist();
        IEnumerable<CardStack> GetAllRecords();
        bool Insert(CardStack entity);
        bool Update(CardStack entity);
    }
}