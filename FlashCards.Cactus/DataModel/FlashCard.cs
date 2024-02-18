namespace FlashCards.Cactus.DataModel
{
    public class FlashCard
    {
        #region Constructors

        public FlashCard() { }

        public FlashCard(int id, int sId, string front, string back)
        {
            Id = id;
            SId = sId;
            Front = front;
            Back = back;
        }

        public FlashCard(int sId, string front, string back)
        {
            SId = sId;
            Front = front;
            Back = back;
        }

        #endregion Constructors

        #region Properties

        public int Id { get; set; }

        public int SId { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }

        #endregion Properties
    }
}
