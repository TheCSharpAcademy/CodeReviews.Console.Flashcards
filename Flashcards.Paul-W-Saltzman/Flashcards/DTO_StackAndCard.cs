
namespace Flashcards
{
    internal class DtoStackAndCard
    {

        internal int StackID;
        internal String StackName;
        internal int CardNumberInStack;
        internal String CardFront;
        internal String CardBack;

        public DtoStackAndCard()
        {

        }


        public DtoStackAndCard(Card card, Stack stack)
        {
            StackID = stack.StackID;
            StackName = stack.StackName;
            CardNumberInStack = card.NoInStack;
            CardFront = card.Front;
            CardBack = card.Back;
        }

        public static List<DtoStackAndCard> LoadStackAndCardList(Stack selectedStack)
        {
            List <DtoStackAndCard> studySession = new List<DtoStackAndCard>();
            List<Card> cards = Data.LoadCards(selectedStack.StackID);
            foreach (Card card in cards) 
            {
                DtoStackAndCard stackAndCard = new DtoStackAndCard(card, selectedStack);
                studySession.Add(stackAndCard);
            }

            return studySession;
        }

       
    }
}
