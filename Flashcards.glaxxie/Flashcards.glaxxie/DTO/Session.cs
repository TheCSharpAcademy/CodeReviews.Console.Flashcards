namespace Flashcards.glaxxie.DTO;

internal record SessionCreation(int StackId, DateTime Date, int Score);
internal record SessionViewer(int SessionId, int StackId, DateTime Date, int Score);
//internal record SessionReader(string Stack, DateTime Date, int Score);

//internal record CardCreation(int StackId, string Front, string Back);
//internal record CardModification(int CardId, string Front, string Back);
//internal record CardReader(int CardId, int StackId, string Front, string Back);

//// For reading data (includes ID)
//internal record CardDto(int CardId, string Front, string Back, int StackId);
//internal record StackDto(int StackId, string StackName);
//internal record SessionDto(int SessionId, DateTime Date, int Score, int StackId);

//// For inserting new data (no ID yet)
//internal record CreateCardDto(string Front, string Back, int StackId);
//internal record CreateStackDto(string StackName);
//internal record CreateSessionDto(DateTime Date, int Score, int StackId);

//// For updating existing records
//internal record UpdateCardDto(int CardId, string Front, string Back);
//internal record UpdateStackDto(int StackId, string StackName);
//internal record UpdateSessionDto(int SessionId, DateTime Date, int Score);

////Perfect.In that case, your DTO setup can be:

////    Card → 3 DTOs: CreateCardDto, CardDto, UpdateCardDto

////    Stack → 2 DTOs: CreateStackDto, StackDto

////    Session → 2 DTOs: CreateSessionDto, SessionDto