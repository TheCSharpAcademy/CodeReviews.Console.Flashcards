namespace Flashcards.jkjones98;

internal class StartStudySession
{
    StudySessionController controller = new();
    internal void StartSession(int stackId, int sessionLength)
    {
        List<FlashcardDto> flashcards = controller.GetFlashcards(stackId);
        int sessionScore = 0;

        for (int i = 0; i < sessionLength; i++)
        {
            var random = new Random();
            int index = random.Next(flashcards.Count);
            List<StudyCardDto> showCard = new List<StudyCardDto>();
            showCard.Add(new StudyCardDto
            {
                FlashcardId = flashcards[index].FlashcardId,
                Front = flashcards[index].Front,
            });
            ShowTable.CreateSessionTable(showCard);

            Console.WriteLine("\nPlease enter the meaning of this word");
            string userAnswer = Console.ReadLine();
            if(userAnswer.ToLower() == flashcards[index].Back.ToLower()) 
            {   
                sessionScore++;
            }

            Console.Clear();
        }
        
        Console.WriteLine($"\nYou scored {sessionScore} out of {sessionLength}, well done!");
        string studyLang = controller.GetLanguageName(stackId);
        DateTime dateToParse = DateTime.Now;
        string date = dateToParse.Date.ToString("yyyy-MM-dd");
        StudySession studySession = new StudySession{Date = date, Score = sessionScore, Studied = sessionLength, Language = studyLang, StackId = stackId};

        controller.InsertStudyDb(studySession);
    }
}