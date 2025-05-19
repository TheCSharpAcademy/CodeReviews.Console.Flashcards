namespace Flashcards.glaxxie.DTO;

internal record CardCreation(int StackId, string Front, string Back)
{
    override public string  ToString() => (StackId == -1) ? "Back" : $"Stack {StackId} : {Front} => {Back}";
};
internal record CardModification(int CardId, string Front, string Back);

internal record CardViewer(int CardId = -1, int StackId = -1, string Front = "Front", string Back = "Back")
{
    override public string ToString() => (StackId == -1) ? "Back" : $"Card. {CardId} : {Front} => {Back}";
}

internal record CardDisplay(int Seq, string StackName , string Front, string Back);