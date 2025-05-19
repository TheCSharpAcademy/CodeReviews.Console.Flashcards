namespace Flashcards.glaxxie.DTO;

internal record SessionCreation(int StackId, DateTime Date, int Cards, int Score);
internal record SessionViewer(int SessionId, string StackName, DateTime Date, int Cards, int Score);