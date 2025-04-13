namespace SpirosZoupas.Console.Flashcards.DAL.DTO
{
    public class FlashcardStackDTO
    {
        public int ID { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
        public string StackName { get; set; }
    }
}
// using counter either in repo when selecting the data & before adding it to the list
// or when displaying while looping through the resulted List<FLashcardStackDTO> use counter
// to display  numbers should start with 1