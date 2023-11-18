
namespace Flashcards
{
    internal class Card
    {
        internal int CardID { get; set; }
        internal int StackID { get; set; }
        internal int NoInStack { get; set; }
        internal string? Front { get; set; }
        internal string? Back { get; set; }
        internal Card() { }

        internal static Card NewCard(int stackID, string front, string back)
        {
            Card creatingCard = new Card();
            creatingCard.StackID = stackID;
            creatingCard.NoInStack = Card.NextNoInStack(creatingCard.StackID);
            creatingCard.Front = front;
            creatingCard.Back = back;
            creatingCard.CardID = Data.EnterCard(creatingCard.StackID, creatingCard.NoInStack, creatingCard.Front, creatingCard.Back);
            return creatingCard;
        }

        internal static int NextNoInStack(int stackID)
        {
            int noInStack = 0;
            List<Card> cards = Data.LoadCards(stackID);
            if (cards.Count == 0)
            {
                noInStack = 1;
            }
            else if (cards.Count > 0)
            {
                noInStack = cards.Count + 1;
            }
            return noInStack;
        }

        internal static void ReNumberCardsInStack(int stackId)
        {
            List<Card> cards = Data.LoadCards(stackId);
            List<Card> sortedCards = cards.OrderBy(o => o.CardID).ToList();

            int i = 1;
            foreach (Card card in sortedCards)
            {
                card.NoInStack = i;
                i++;
                Data.UpdateCard(card);

            }

        }

        internal static void LoadSeedDataCards()
        {
            NewCard(1, "string", "sequence of characters used to represent text");
            NewCard(1, "int", "number");
            NewCard(1, "bool", "true/false");
            NewCard(1, "double", "double precision 64 bit");
            NewCard(2, "CCD", "Cash Concentration or Disbursement - Commercial");
            NewCard(2, "WEB", "Internet Initiated Transaction - Commercial/Retail");
            NewCard(2, "PPD", "Prearranged Payment and Deposit - Retail");
            NewCard(3, "Bonjour", "Hello");
            NewCard(3, "Salut", "Bye");
            NewCard(3, "Oui", "Yes");
            NewCard(4, "Chao", "Hello");
            NewCard(4, "Tam biet", "Bye");
            NewCard(4, "Co", "Yes");
            NewCard(5, "Hola", "Hello");
            NewCard(5, "adios", "Bye");
            NewCard(5, "Si", "Yes");

        }
    }
}
