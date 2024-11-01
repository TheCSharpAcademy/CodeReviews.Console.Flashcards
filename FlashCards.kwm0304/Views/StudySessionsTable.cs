using FlashCards.kwm0304.Models;
using FlashCards.kwm0304.Repositories;
using FlashCards.kwm0304.Services;
using Spectre.Console;

namespace FlashCards.kwm0304.Views;

public class StudySessionsTable(StackService service, StudySessionRepository repository)
{
  private readonly StudySessionRepository _repository = repository;
  private readonly StackService _stackService = service;

    public async Task<List<StudySession>> GetAllStudySessionsAsync()
  {
    return await _repository.GetAllSessionsAsync();
  }

  private static List<int> ExtractStackIds(List<StudySession> sessions)
  {
    return sessions.Select(s => s.StackId).Distinct().ToList();
  }

  private async Task<List<string>> GetStackNamesAsync(List<int> stackIds)
  {
    return await _stackService.GetAllStackNames(stackIds);
  }

  private static Dictionary<int, string> CreateStackIdToNameMap(List<int> stackIds, List<string> stackNames)
  {
    Dictionary<int, string> mapIdToName = new Dictionary<int, string>();
    for (int i = 0; i < stackIds.Count; i++)
    {
      mapIdToName[stackIds[i]] = stackNames[i];
    }
    return mapIdToName;
  }

  public async Task AllSessionsTable()
  {
    List<StudySession> sessions = await GetAllStudySessionsAsync();
    if (sessions == null || sessions.Count == 0)
    {
      AnsiConsole.WriteLine("No study sessions to display");
      return;
    }

    List<int> stackIds = ExtractStackIds(sessions);
    List<string> stackNames = await GetStackNamesAsync(stackIds);
    Dictionary<int, string> mapIdToName = CreateStackIdToNameMap(stackIds, stackNames);

        DisplaySessionsTable(sessions, mapIdToName);
  }

  private static void DisplaySessionsTable(List<StudySession> sessions, Dictionary<int, string> mapIdToName)
  {
    string[] cols = ["Studied At", "Stack", "Score"];
    var table = new Table();
    table.Title("All Study Sessions");
    table.AddColumns(cols);
    foreach (var session in sessions)
    {
      table.AddRow(
          session.StudiedAt.ToString("g"),
          mapIdToName.TryGetValue(session.StackId, out var stackName) ? stackName : "Unknown",
          session.Score.ToString()
      );
    }
    AnsiConsole.Write(table);
    Console.WriteLine("\nPress any key to return to the main menu...");
    Console.ReadKey(true);
  }
}