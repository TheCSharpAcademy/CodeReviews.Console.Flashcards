

namespace Flashcards.Bina28.Models;

internal class StudySessionDto
{    public int SessionId { get; set; }
	public DateTime SessionDate {  get; set; }
	public string StackName { get; set; }
	public int NumberOfWrongAnswers { get; set; }
	public int NumberOfRightAnswers { get; set; }
	public int TotalQuestions { get; set; }

	public StudySessionDto(DateTime date, string stackname, int wrongAnswer, int rightAnswer, int total) { 
		SessionDate = date;
		StackName = stackname;
		NumberOfWrongAnswers = wrongAnswer;
		NumberOfRightAnswers = rightAnswer;
		TotalQuestions = total;
	}
}
