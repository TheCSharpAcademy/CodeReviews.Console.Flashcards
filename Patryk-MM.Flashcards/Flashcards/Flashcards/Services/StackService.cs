using Flashcards.Repositories;
using Spectre.Console;

namespace Flashcards.Services;
public class StackService {
    private readonly IStackRepository _repository;

    public StackService(IStackRepository repository) {
        _repository = repository;
    }

    public async Task ViewStacks() {
        var stackList = await _repository.GetStackNamesAsync();
        foreach (var stackName in stackList) {
            AnsiConsole.WriteLine(stackName);
        }
    }
}

