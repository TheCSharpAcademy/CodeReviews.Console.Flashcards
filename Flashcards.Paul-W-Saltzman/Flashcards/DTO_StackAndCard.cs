
namespace Flashcards
{
    internal class DTO_StackAndCard
    {

        internal int StackID;
        internal String StackName;
        internal int CardNumberInStack;
        internal String CardFront;
        internal String CardBack;

        public DTO_StackAndCard()
        {

        }


        public DTO_StackAndCard(Card card, Stack stack)
        {
            StackID = stack.StackID;
            StackName = stack.StackName;
            CardNumberInStack = card.NoInStack;
            CardFront = card.Front;
            CardBack = card.Back;
        }

        public static List<DTO_StackAndCard> LoadStackAndCardList(Stack selectedStack)
        {
            List <DTO_StackAndCard> studySession = new List<DTO_StackAndCard>();
            List<Card> cards = Data.LoadCards(selectedStack.StackID);
            foreach (Card card in cards) 
            {
                DTO_StackAndCard stackAndCard = new DTO_StackAndCard(card, selectedStack);
                studySession.Add(stackAndCard);
            }

            return studySession;
        }

       
    }
}
